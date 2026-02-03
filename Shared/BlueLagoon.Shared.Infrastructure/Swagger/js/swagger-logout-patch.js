(function () {
    const initPatch = () => {
        console.log("Swagger Logout Patch: Inicjalizacja...");

        const patchLogout = () => {
            const allButtons = document.querySelectorAll('button');

            allButtons.forEach(btn => {
                if (btn.textContent.trim() === "Logout" && !btn.getAttribute('data-logout-patched')) {
                    btn.setAttribute('data-logout-patched', 'true');

                    btn.style.border = "3px solid red";
                    console.log("Swagger Logout Patch: Znaleziono przycisk Logout, podpinam zdarzenie.");

                    btn.addEventListener('click', function (e) {
                        console.log("Swagger Logout Patch: Przechwycono kliknięcie!");

                        const logoutUrl = window.location.origin + '/connect/logout?post_logout_redirect_uri=' + encodeURIComponent(window.location.href);
                        window.location.href = logoutUrl;
                    }, { capture: true });
                }
            });
        };

        const targetNode = document.body;
        if (targetNode) {
            const observer = new MutationObserver(() => patchLogout());
            observer.observe(targetNode, { childList: true, subtree: true });
            patchLogout();
        }
    };

    if (document.readyState === "complete" || document.readyState === "interactive") {
        initPatch();
    } else {
        document.addEventListener("DOMContentLoaded", initPatch);
    }
})();