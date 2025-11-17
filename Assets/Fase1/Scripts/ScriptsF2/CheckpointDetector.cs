using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class CheckpointDetector : MonoBehaviour
{
    public TimeBarController timeBar;     // arraste a barra de tempo aqui
    public VideoClip cutsceneFase3;       // arraste o vídeo da fase 3

    public GameObject menuCanvas;         // arraste o Canvas do Menu Principal

    private bool venceu = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCar") && !venceu)
        {
            venceu = true;

            // para o timer imediatamente (sem game over)
            if (timeBar != null)
            {
                timeBar.StopTimerAfterWin();
            }

            Debug.Log("Checkpoint alcançado! VENCEDOR! Iniciando cutscene final...");

            StartCoroutine(PlayFinalCutsceneSequence());
        }
    }

    IEnumerator PlayFinalCutsceneSequence()
    {
        // espera 3 segundos antes da cutscene
        yield return new WaitForSeconds(3f);

        if (CutscenePlayer.Instance != null)
        {
            // toca cutscene fase 3
            CutscenePlayer.Instance.PlayCutscene(cutsceneFase3, null);
        }

        // espera a duração do vídeo
        if (cutsceneFase3 != null)
            yield return new WaitForSeconds((float)cutsceneFase3.length);

        // ativa menu e pausa o jogo
        if (menuCanvas != null)
            menuCanvas.SetActive(true);

        Time.timeScale = 0f;

        Debug.Log("Cutscene encerrada – retornando ao menu.");

        // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        // ADIÇÃO: reset total do jogo chamando o MenuManager
        MenuManager menu = FindObjectOfType<MenuManager>();
        if (menu != null)
        {
            // CHAMA A FUNÇÃO ResetarJogoCompleto() SE EXISTIR
            var metodo = menu.GetType().GetMethod("ResetarJogoCompleto");
            if (metodo != null)
            {
                metodo.Invoke(menu, null);
            }
        }
        // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    }
}
