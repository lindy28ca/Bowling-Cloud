using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Paneles
    [SerializeField] private GameObject panelPrincipal;
    [SerializeField] private GameObject panelJugadorCount;
    [SerializeField] private GameObject panelNombres;

    // Controles del panel de cantidad de jugadores
    [SerializeField] private InputField inputPlayerCount;
    [SerializeField] private Button aceptarPlayerCountButton;

    // Controles del panel de nombres
    [SerializeField] private InputField[] playerNameInputs;
    [SerializeField] private Button comenzarJuegoButton;

    private int playerCount = 1;

    void Start()
    {
        panelPrincipal.SetActive(true);
        panelJugadorCount.SetActive(false);
        panelNombres.SetActive(false);

        aceptarPlayerCountButton.onClick.AddListener(AceptarCantidadJugadores);
        comenzarJuegoButton.onClick.AddListener(ComenzarJuego);
    }

    public void Jugar()
    {
        panelPrincipal.SetActive(false);
        panelJugadorCount.SetActive(true);
    }

    void AceptarCantidadJugadores()
    {
        playerCount = int.Parse(inputPlayerCount.text);

        if (playerCount < 1)
        {
            playerCount = 1;
        }
        if (playerCount > 8)
        {
            playerCount = 8;
        }

        panelJugadorCount.SetActive(false);
        panelNombres.SetActive(true);

        int i = 0;
        while (i < playerNameInputs.Length)
        {
            if (i < playerCount)
            {
                playerNameInputs[i].gameObject.SetActive(true);
            }
            else
            {
                playerNameInputs[i].gameObject.SetActive(false);
            }
            i++;
        }
    }
    void ComenzarJuego()
    {
        PlayerPrefs.SetInt("PlayerCount", playerCount);

        int i = 0;
        while (i < playerCount)
        {
            PlayerPrefs.SetString("PlayerName" + i, playerNameInputs[i].text);
            i++;
        }

        SceneManager.LoadScene("GameScene");
    }
    public void Salir()
    {
        Application.Quit();
    }
}
