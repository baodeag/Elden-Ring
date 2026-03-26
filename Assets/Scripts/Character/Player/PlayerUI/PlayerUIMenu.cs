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
            return menu != null && menu.activeInHierarchy;
        }

        public virtual void OpenMenu()
        {
            menu.SetActive(true);
            PlayerUIManager.instance.RefreshMenuWindowState();
        }

        public virtual void OpenMenuAfterFixedFrame()
        {
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
            menu.SetActive(false);
            PlayerUIManager.instance.RefreshMenuWindowState();
        }

        public virtual void CloseMenuAfterFixedFrame()
        {
            if (!menu.activeInHierarchy)
                return;

            StartCoroutine(WaitThenCloseMenu());
        }

        protected virtual IEnumerator WaitThenCloseMenu()
        {
            yield return new WaitForFixedUpdate();

            CloseMenu();
        }
    }
}
