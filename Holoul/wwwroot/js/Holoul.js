// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    // Set current year in footer
    document.getElementById("current-year").textContent =
        new Date().getFullYear();

    // Header scroll effect
    const header = document.querySelector(".header");
    function handleScroll() {
        if (window.scrollY > 20) {
            header.classList.add("scrolled");
        } else {
            header.classList.remove("scrolled");
        }
    }
    window.addEventListener("scroll", handleScroll);

    // Reveal animations on scroll
    const observerOptions = {
        threshold: 0.1,
        rootMargin: "0px 0px -100px 0px",
    };

    const revealObserver = new IntersectionObserver((entries) => {
        entries.forEach((entry) => {
            if (entry.isIntersecting) {
                entry.target.classList.add("revealed");
                // Once revealed, no need to observe anymore
                revealObserver.unobserve(entry.target);
            }
        });
    }, observerOptions);

    const elementsToReveal = document.querySelectorAll(
        ".reveal-up, .reveal-left, .reveal-right"
    );
    elementsToReveal.forEach((element) => {
        revealObserver.observe(element);
    });

    // Solutions Slider
    const solutionsSlider = document.querySelector(".solutions-slider");
    const solutionsPrevBtn = document.querySelector(".solutions .slider-prev");
    const solutionsNextBtn = document.querySelector(".solutions .slider-next");

    if (solutionsSlider && solutionsNextBtn && solutionsPrevBtn) {
        const cardWidth = 288; // card width (272) + gap (16)

        solutionsNextBtn.addEventListener("click", () => {
            solutionsSlider.scrollBy({ left: cardWidth, behavior: "smooth" });
        });

        solutionsPrevBtn.addEventListener("click", () => {
            solutionsSlider.scrollBy({ left: -cardWidth, behavior: "smooth" });
        });
    }

    // Testimonials Slider
    const testimonialsSlider = document.querySelector(".testimonials-slider");
    const testimonialsPrevBtn = document.querySelector(
        ".testimonials .slider-prev"
    );
    const testimonialsNextBtn = document.querySelector(
        ".testimonials .slider-next"
    );

    if (testimonialsSlider && testimonialsNextBtn && testimonialsPrevBtn) {
        const cardWidth = 366; // card width (350) + gap (16)

        testimonialsNextBtn.addEventListener("click", () => {
            testimonialsSlider.scrollBy({ left: cardWidth, behavior: "smooth" });
        });

        testimonialsPrevBtn.addEventListener("click", () => {
            testimonialsSlider.scrollBy({ left: -cardWidth, behavior: "smooth" });
        });
    }

    // Mobile menu toggle (simplified version without showing the actual menu)
    const mobileMenuBtn = document.querySelector(".mobile-menu-btn");
    if (mobileMenuBtn) {
        mobileMenuBtn.addEventListener("click", () => {
            alert("Mobile menu would open here in a complete implementation.");
        });
    }

    // Newsletter form submission
    const newsletterForm = document.querySelector(".newsletter-form");
    if (newsletterForm) {
        newsletterForm.addEventListener("submit", (e) => {
            e.preventDefault();
            const emailInput = newsletterForm.querySelector('input[type="email"]');
            if (emailInput && emailInput.value) {
                alert(`Thank you for subscribing with: ${emailInput.value}`);
                emailInput.value = "";
            }
        });
    }

    // Smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach((anchor) => {
        anchor.addEventListener("click", function (e) {
            const href = this.getAttribute("href");

            if (href !== "#") {
                e.preventDefault();

                const targetElement = document.querySelector(href);
                if (targetElement) {
                    targetElement.scrollIntoView({
                        behavior: "smooth",
                    });
                }
            }
        });
    });
});

document.addEventListener("DOMContentLoaded", function () {
    // Function to animate the counter
    function animateCounter(element, start, end, duration) {
        let startTimestamp = null;
        const step = (timestamp) => {
            if (!startTimestamp) startTimestamp = timestamp;
            const progress = Math.min((timestamp - startTimestamp) / duration, 1);
            const currentNumber = Math.floor(progress * (end - start) + start);
            element.textContent = currentNumber.toLocaleString(); // Adds commas for thousands
            if (progress < 1) {
                requestAnimationFrame(step);
            }
        };
        requestAnimationFrame(step);
    }

    // Intersection Observer to detect when the section is in view
    const observer = new IntersectionObserver(
        (entries, observer) => {
            entries.forEach((entry) => {
                if (entry.isIntersecting) {
                    const element = entry.target;
                    element.classList.add("active"); // Reveal the section

                    const counter = element.querySelector(".counter");
                    if (counter && !counter.classList.contains("counted")) {
                        const targetNumber = parseInt(counter.getAttribute("data-number"));
                        animateCounter(counter, 0, targetNumber, 2000); // 2000ms = 2 seconds
                        counter.classList.add("counted"); // Prevent re-animation
                    }
                }
            });
        },
        { threshold: 0.5 } // Trigger when 50% of the element is in view
    );

    // Observe all feature-box elements
    const featureBoxes = document.querySelectorAll(".feature-box");
    featureBoxes.forEach((box) => observer.observe(box));
});
