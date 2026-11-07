// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/* ======= DARK / LIGHT MODE TOGGLE ======= */
(function () {
    // Apply saved preference immediately (before DOM paint) to avoid flash
    var saved = localStorage.getItem('themeMode');
    if (saved === 'light') {
        document.documentElement.classList.add('light-mode-preload');
    }

    document.addEventListener('DOMContentLoaded', function () {
        var body = document.body;

        // Restore saved theme
        if (localStorage.getItem('themeMode') === 'light') {
            body.classList.add('light-mode');
        }

        function syncIcons() {
            var isLight = body.classList.contains('light-mode');
            // Desktop icon
            var icon = document.getElementById('themeIcon');
            if (icon) {
                icon.className = isLight ? 'fas fa-moon' : 'fas fa-sun';
            }
            // Mobile icon
            var iconM = document.getElementById('themeIconMobile');
            if (iconM) {
                iconM.className = isLight ? 'fas fa-moon' : 'fas fa-sun';
            }
        }

        function toggleTheme() {
            body.classList.toggle('light-mode');
            var isLight = body.classList.contains('light-mode');
            localStorage.setItem('themeMode', isLight ? 'light' : 'dark');
            syncIcons();
        }

        // Desktop button
        var btn = document.getElementById('themeToggleBtn');
        if (btn) btn.addEventListener('click', toggleTheme);

        // Mobile button
        var btnM = document.getElementById('themeToggleBtnMobile');
        if (btnM) btnM.addEventListener('click', toggleTheme);

        // Initial icon sync
        syncIcons();
    });
})();

/* ======= SCROLL TO TOP ======= */
(function () {
    document.addEventListener('DOMContentLoaded', function () {
        var btn = document.getElementById('scrollToTopBtn');
        if (!btn) return;

        // Show/hide on scroll
        window.addEventListener('scroll', function () {
            if (window.scrollY > 300) {
                btn.classList.add('show');
            } else {
                btn.classList.remove('show');
            }
        }, { passive: true });

        // Scroll to top on click
        btn.addEventListener('click', function () {
            window.scrollTo({ top: 0, behavior: 'smooth' });
        });
    });
})();
