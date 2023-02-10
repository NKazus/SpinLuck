using UnityEngine ;
using UnityEngine.UI ;
using DG.Tweening ;
using System.Collections.Generic ;
using System;
using TMPro;
using Zenject;

public class PickerWheel : MonoBehaviour
{
    [Header("References :")]
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Transform linesParent;

    [Space(5)]
    [SerializeField] private Transform PickerWheelTransform;
    [SerializeField] private Transform wheelCircle;
    [SerializeField] private GameObject wheelPiecePrefab;
    [SerializeField] private Transform wheelPiecesParent;

    [Space(5)]
    [SerializeField] private Transform winPiece;

    [Space(5)]
    [Header("Picker wheel settings :")]
    [Range(1, 20)][SerializeField] private int spinDuration = 8;

    private List<WheelPiece> wheelPieces;

    private bool isSpinning = false;

    private Vector2 pieceMinSize = new Vector2(80f, 140f);
    private Vector2 pieceMaxSize = new Vector2(140f, 210f);
    private int piecesMin = 2;
    private int piecesMax = 8;

    private float pieceAngle;
    private float halfPieceAngle;
    private float halfPieceAngleWithPaddings;

    private double accumulatedWeight;
    private System.Random rand = new System.Random();

    private List<int> nonZeroChancesIndices = new List<int>();

    [Inject] private readonly GlobalEventManager eventManager;
    [Inject] private readonly GlobalResourceManager resourceManager;

    private void Start()
    {
        GetWheelPieces();
        SetupPieces();

        pieceAngle = 360 / wheelPieces.Count;
        halfPieceAngle = pieceAngle / 2f;
        halfPieceAngleWithPaddings = halfPieceAngle - (halfPieceAngle / 4f);

        Generate();

        if (nonZeroChancesIndices.Count == 0)
        {
            Debug.LogError("You can't set all pieces chance to zero");
        }
    }

    private void OnDisable()
    {
        DOTween.KillAll(this);
    }

    private void GetWheelPieces()
    {
        wheelPieces = resourceManager.GetResources();
    }

    private void SetupPieces()
    {
        for (int i = 0; i < wheelPieces.Count; i++)
        {
            wheelPieces[i].Chance = rand.NextFloat(100f);
            wheelPieces[i].Amount = rand.Next(14) + 1;
        }
        CalculateWeights();
    }


    private void Generate()
    {
        wheelPiecePrefab = InstantiatePiece();

        RectTransform rt = wheelPiecePrefab.transform.GetChild(0).GetComponent<RectTransform>();
        float pieceWidth = Mathf.Lerp(pieceMinSize.x, pieceMaxSize.x, 1f - Mathf.InverseLerp(piecesMin, piecesMax, wheelPieces.Count));
        float pieceHeight = Mathf.Lerp(pieceMinSize.y, pieceMaxSize.y, 1f - Mathf.InverseLerp(piecesMin, piecesMax, wheelPieces.Count));
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pieceWidth);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pieceHeight);

        for (int i = 0; i < wheelPieces.Count; i++)
        {
            DrawPiece(i);
        }

        Destroy(wheelPiecePrefab);
    }

    private void DrawPiece(int index)
    {
        WheelPiece piece = wheelPieces[index];
        Transform pieceTrns = InstantiatePiece().transform.GetChild(0);

        pieceTrns.GetChild(0).GetComponent<Image>().sprite = piece.Icon;
        pieceTrns.GetChild(1).GetComponent<TextMeshProUGUI>().text = piece.Amount.ToString();

        Transform lineTrns = Instantiate(linePrefab, linesParent.position, Quaternion.identity, linesParent).transform;
        lineTrns.RotateAround(wheelPiecesParent.position, Vector3.back, (pieceAngle * index));

        pieceTrns.RotateAround(wheelPiecesParent.position, Vector3.back, (pieceAngle * index) + halfPieceAngle);
    }

    private GameObject InstantiatePiece()
    {
        return Instantiate(wheelPiecePrefab, wheelPiecesParent.position, Quaternion.identity, wheelPiecesParent);
    }

    private int GetRandomPieceIndex()
    {
        double r = rand.NextDouble() * accumulatedWeight;
        for (int i = 0; i < wheelPieces.Count; i++)
        {
            if (wheelPieces[i].Weight >= r)
            {
                return i;
            }
        }
        return 0;
    }

    private void CalculateWeights()
    {
        for (int i = 0; i < wheelPieces.Count; i++)
        {
            WheelPiece piece = wheelPieces[i];

            accumulatedWeight += piece.Chance;
            piece.Weight = accumulatedWeight;

            if (piece.Chance > 0)
            {
                nonZeroChancesIndices.Add(i);
            }
        }
    }

    public void Spin()
    {
        if (!isSpinning)
        {
            isSpinning = true;
            eventManager.StartSpinning();

            int index = GetRandomPieceIndex();
            WheelPiece piece = wheelPieces[index];

            if (piece.Chance == 0 && nonZeroChancesIndices.Count != 0)
            {
                index = nonZeroChancesIndices[rand.Next(nonZeroChancesIndices.Count)];
                piece = wheelPieces[index];
            }

            float angle = -(pieceAngle * index + halfPieceAngle);

            float rightOffset = (angle - halfPieceAngleWithPaddings) % 360;
            float leftOffset = (angle + halfPieceAngleWithPaddings) % 360;

            float randomAngle = rand.NextFloat(leftOffset, rightOffset);

            Vector3 targetRotation = Vector3.back * (randomAngle + 2 * 360 * spinDuration);

            float prevAngle, currentAngle;
            prevAngle = currentAngle = wheelCircle.eulerAngles.z;

            bool isIndicatorOnTheLine = false;

            wheelCircle
                .DORotate(targetRotation, spinDuration, RotateMode.Fast)
                .SetId(this)
                .SetEase(Ease.InOutQuart)
                .OnUpdate(() => {
                    float diff = Mathf.Abs(prevAngle - currentAngle);
                    if (diff >= halfPieceAngle)
                    {
                        prevAngle = currentAngle;
                        isIndicatorOnTheLine = !isIndicatorOnTheLine;
                    }
                    currentAngle = wheelCircle.eulerAngles.z;
                })
                .OnKill(() => {
                    isSpinning = false;

                    winPiece.gameObject.SetActive(true);
                    winPiece.GetChild(0).GetComponent<Image>().sprite = piece.Icon;
                    winPiece.GetChild(1).GetComponent<TextMeshProUGUI>().text = piece.Amount.ToString();
                    eventManager.StopSpinning(piece);
                });

        }
    }

}