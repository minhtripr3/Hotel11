document.addEventListener("DOMContentLoaded", function () {
    // Image Thumbnail Click Event
    const thumbs = document.querySelectorAll('.product-thumbs .pt');
    const bigImage = document.querySelector('.product-big-img');

    thumbs.forEach(thumb => {
        thumb.addEventListener('click', function () {
            const bigImageUrl = thumb.getAttribute('data-imgbigurl');
            bigImage.src = bigImageUrl;
            thumbs.forEach(t => t.classList.remove('active'));
            thumb.classList.add('active');
        });
    });

    // Tabs Navigation
    const tabs = document.querySelectorAll('.tab-item ul.nav li a');
    const tabContents = document.querySelectorAll('.tab-content .tab-pane');

    tabs.forEach(tab => {
        tab.addEventListener('click', function (event) {
            event.preventDefault();
            tabs.forEach(t => t.classList.remove('active'));
            tab.classList.add('active');
            const target = tab.getAttribute('href');
            tabContents.forEach(tc => {
                tc.classList.remove('fade-in', 'active');
                if (tc.getAttribute('id') === target.substring(1)) {
                    tc.classList.add('fade-in', 'active');
                }
            });
        });
    });
});