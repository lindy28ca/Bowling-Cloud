using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;    

public class GameManager : MonoBehaviour
{
    [Header("Jugadores")]
    public TextMeshProUGUI turnText; 
    public TextMeshProUGUI scoreText; 
    private List<string> playerNames = new List<string>();
    private List<int> playerScores = new List<int>();
    private int currentPlayer = 0;

    [Header("Referencias")]
    [SerializeField] private BallControler ball;
    [SerializeField] private List<Pin> pins = new List<Pin>(); 
    private Vector3[] pinPositions;

    [Header("Configuración de Partida")]
    [SerializeField] private int totalRondas; 
    [SerializeField] private GameObject panelGanador;
    [SerializeField] private TextMeshProUGUI textoGanador;

    private int rondaActual = 1;
    private int tirosRestantes = 2;


    void Start()
    {
        int count = PlayerPrefs.GetInt("PlayerCount", 1);

        for (int i = 0; i < count; i++)
        {
            string name = PlayerPrefs.GetString("PlayerName" + i, "Jugador" + (i + 1));
            playerNames.Add(name);
            playerScores.Add(0);
        }

        pinPositions = new Vector3[pins.Count];
        for (int i = 0; i < pins.Count; i++)
        {
            pinPositions[i] = pins[i].transform.position;
        }

        ActualizarUI();
    }
    public void RevisarTiro()
    {
        int knockedThisThrow = 0;
        for (int i = 0; i < pins.Count; i++)
        {
            if (pins[i].Cayo() && pins[i].gameObject.activeSelf)
            {
                knockedThisThrow++;
            }
        }

        playerScores[currentPlayer] += knockedThisThrow;
        tirosRestantes--;

        if (knockedThisThrow == pins.Count && tirosRestantes == 1)
        {
            tirosRestantes = 0;
        }

        if (tirosRestantes > 0)
        {
            ResetearSoloCaidos();
        }
        else
        {
            tirosRestantes = 2;
            ResetearPinos();
            SiguienteJugador();

            if (currentPlayer == 0)
            {
                rondaActual++;
                if (rondaActual > totalRondas)
                {
                    TerminarPartida();
                    return;
                }
            }
        }

        ball.ResetMovimiento();
        ActualizarUI();
    }

    void SiguienteJugador()
    {
        currentPlayer++;
        if (currentPlayer >= playerNames.Count)
        {
            currentPlayer = 0;
        }
    }
    void ActualizarUI()
    {
        turnText.text = "Ronda " + rondaActual + "/" + totalRondas +
                        " - Turno: " + playerNames[currentPlayer] +
                        " (Tiros restantes: " + tirosRestantes + ")";

        scoreText.text = "Puntajes:\n";
        for (int i = 0; i < playerNames.Count; i++)
        {
            scoreText.text += playerNames[i] + ": " + playerScores[i] + "\n";
        }
    }
    void ResetearPinos()
    {
        for (int i = 0; i < pins.Count; i++)
        {
            pins[i].gameObject.SetActive(true);
            pins[i].Resetear(pinPositions[i]);
        }
    }
    void ResetearSoloCaidos()
    {
        for (int i = 0; i < pins.Count; i++)
        {
            if (pins[i].Cayo())
            {
                pins[i].gameObject.SetActive(false);
            }
        }
    }

    void TerminarPartida()
    {
        turnText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        panelGanador.SetActive(true);

        int maxScore = -1;
        string ganador = "";

        for (int i = 0; i < playerNames.Count; i++)
        {
            if (playerScores[i] > maxScore)
            {
                maxScore = playerScores[i];
                ganador = playerNames[i];
            }
        }

        textoGanador.text = "¡Ganador: " + ganador + " con " + maxScore + " puntos!";
    }
}
