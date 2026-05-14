using UnityEngine;
using System.Collections;

namespace baodeag
{
    public class PlayerUIMenu : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] protected GameObject menu;

        public bool IsMenuOpen()
        {
            EnsureMenuReference();
            return gameObject.activeInHierarchy && menu != null && menu.activeInHierarchy;
        }

        public virtual void OpenMenu()
        {
            EnsureMenuReference();

            if (menu == null)
                return;

            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            menu.SetActive(true);
            if (PlayerUIManager.instance != null)
                PlayerUIManager.instance.RefreshMenuWindowState();
        }

        public virtual void OpenMenuAfterFixedFrame()
        {
            EnsureMenuReference();

            if (menu == null)
                return;

            if (menu.activeInHierarchy)
                return;

            StartCoroutine(WaitThenOpenMenu());
        }

        protected virtual IEnumerator WaitThenOpenMenu()
        {
            yield return new WaitForFixedUpdate();

            OpenMenu();
        }

        public virtual void CloseMenu()
        {
            EnsureMenuReference();

            if (menu == null)
                return;

            menu.SetActive(false);

            if (menu != gameObject && gameObject.activeSelf)
                gameObject.SetActive(false);

            if (PlayerUIManager.instance != null)
                PlayerUIManager.instance.RefreshMenuWindowState();
        }

        public virtual void CloseMenuAfterFixedFrame()
        {
            EnsureMenuReference();

            if (menu == null)
                return;

            if (!menu.activeInHierarchy)
                return;

            StartCoroutine(WaitThenCloseMenu());
        }

        protected virtual IEnumerator WaitThenCloseMenu()
        {
            yield return new WaitForFixedUpdate();

            CloseMenu();
        }

        protected virtual void EnsureMenuReference()
        {
            if (menu == null)
                menu = gameObject;
        }
    }
}
