// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/* ======= DARK / LIGHT MODE TOGGLE ======= */
(function () {
    var saved = localStorage.getItem('themeMode');
    if (saved === 'light') {
        document.documentElement.classList.add('light-mode-preload');
    }

    document.addEventListener('DOMContentLoaded', function () {
        var body = document.body;

        if (localStorage.getItem('themeMode') === 'light') {
            body.classList.add('light-mode');
        }

        function syncIcons() {
            var isLight = body.classList.contains('light-mode');
            var icon  = document.getElementById('themeIcon');
            var iconM = document.getElementById('themeIconMobile');
            if (icon)  icon.className  = isLight ? 'fas fa-moon' : 'fas fa-sun';
            if (iconM) iconM.className = isLight ? 'fas fa-moon' : 'fas fa-sun';
        }

        function toggleTheme() {
            body.classList.toggle('light-mode');
            var isLight = body.classList.contains('light-mode');
            localStorage.setItem('themeMode', isLight ? 'light' : 'dark');
            syncIcons();
        }

        var btn  = document.getElementById('themeToggleBtn');
        var btnM = document.getElementById('themeToggleBtnMobile');
        if (btn)  btn.addEventListener('click', toggleTheme);
        if (btnM) btnM.addEventListener('click', toggleTheme);
        syncIcons();
    });
})();

/* ======= SCROLL TO TOP ======= */
(function () {
    document.addEventListener('DOMContentLoaded', function () {
        var btn = document.getElementById('scrollToTopBtn');
        if (!btn) return;

        window.addEventListener('scroll', function () {
            btn.classList.toggle('show', window.scrollY > 300);
        }, { passive: true });

        btn.addEventListener('click', function () {
            window.scrollTo({ top: 0, behavior: 'smooth' });
        });
    });
})();

/* =====================================================
   ✨ ROYAL ANIMATIONS — Luxury perfume experience
   ===================================================== */

/* ── 1. GOLD PARTICLES in hero ── */
(function () {
    document.addEventListener('DOMContentLoaded', function () {
        var container = document.querySelector('.hero-particles');
        if (!container) return;

        var COLORS = ['#c8922a', '#e8c87a', '#f5d78e', '#fff8e7'];
        var COUNT  = 28;

        for (var i = 0; i < COUNT; i++) {
            (function () {
                var p = document.createElement('span');
                p.className = 'gold-particle';

                var size   = Math.random() * 5 + 2;
                var left   = Math.random() * 100;
                var delay  = Math.random() * 8;
                var dur    = Math.random() * 6 + 6;
                var color  = COLORS[Math.floor(Math.random() * COLORS.length)];
                var shape  = Math.random() > 0.5 ? '50%' : '2px';

                p.style.cssText = [
                    'position:absolute',
                    'width:'  + size + 'px',
                    'height:' + size + 'px',
                    'background:' + color,
                    'border-radius:' + shape,
                    'left:' + left + '%',
                    'bottom:-10px',
                    'opacity:0',
                    'pointer-events:none',
                    'animation:royalFloat ' + dur + 's ' + delay + 's ease-in infinite'
                ].join(';');

                container.appendChild(p);
            })();
        }
    });
})();

/* ── 2. SCROLL REVEAL (Intersection Observer) ── */
(function () {
    document.addEventListener('DOMContentLoaded', function () {
        if (!('IntersectionObserver' in window)) return;

        var items = document.querySelectorAll(
            '.product-card, .feature-card, .section-header, ' +
            '.hero-stat, .cart-card, .contact-info-card, ' +
            '.rp-card, .contact-form-card, .checkout-card'
        );

        items.forEach(function (el, i) {
            el.classList.add('reveal-hidden');
            el.style.transitionDelay = (i % 6) * 0.08 + 's';
        });

        var observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (e) {
                if (e.isIntersecting) {
                    e.target.classList.add('reveal-visible');
                    observer.unobserve(e.target);
                }
            });
        }, { threshold: 0.12 });

        items.forEach(function (el) { observer.observe(el); });
    });
})();

/* ── 3. 3D TILT on product cards ── */
(function () {
    document.addEventListener('DOMContentLoaded', function () {
        if (window.matchMedia('(hover: none)').matches) return; // skip touch devices

        document.querySelectorAll('.product-card').forEach(function (card) {
            card.addEventListener('mousemove', function (e) {
                var rect   = card.getBoundingClientRect();
                var cx     = rect.left + rect.width  / 2;
                var cy     = rect.top  + rect.height / 2;
                var dx     = (e.clientX - cx) / (rect.width  / 2);
                var dy     = (e.clientY - cy) / (rect.height / 2);
                var rotY   =  dx * 8;
                var rotX   = -dy * 8;
                card.style.transform = 'perspective(700px) rotateX(' + rotX + 'deg) rotateY(' + rotY + 'deg) translateY(-6px)';
            });

            card.addEventListener('mouseleave', function () {
                card.style.transform = '';
                card.style.transition = 'transform 0.5s ease';
                setTimeout(function () { card.style.transition = ''; }, 500);
            });
        });
    });
})();

/* ── 4. SHIMMER on brand name ── */
(function () {
    document.addEventListener('DOMContentLoaded', function () {
        var brands = document.querySelectorAll('.brand-arabic');
        brands.forEach(function (el) {
            el.classList.add('brand-shimmer');
        });
    });
})();

/* ── 5. NUMBER COUNTER animation ── */
(function () {
    document.addEventListener('DOMContentLoaded', function () {
        if (!('IntersectionObserver' in window)) return;

        function animateCounter(el) {
            var text   = el.textContent.trim();
            var prefix = text.match(/^[^\d]*/)[0];
            var suffix = text.match(/[^\d]*$/)[0];
            var num    = parseInt(text.replace(/\D/g, ''), 10);
            if (!num) return;

            var start    = 0;
            var duration = 1800;
            var step     = 16;
            var inc      = num / (duration / step);
            var current  = start;

            var timer = setInterval(function () {
                current += inc;
                if (current >= num) {
                    current = num;
                    clearInterval(timer);
                }
                el.textContent = prefix + Math.floor(current) + suffix;
            }, step);
        }

        var obs = new IntersectionObserver(function (entries) {
            entries.forEach(function (e) {
                if (e.isIntersecting) {
                    animateCounter(e.target);
                    obs.unobserve(e.target);
                }
            });
        }, { threshold: 0.5 });

        document.querySelectorAll('.stat-num').forEach(function (el) {
            obs.observe(el);
        });
    });
})();


