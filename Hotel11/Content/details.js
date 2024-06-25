
// var slide=document.getElementsByClassName('product')[0];

// var nut_trai=document.getElementById('nut_trai');
// var nut_phai=document.getElementById('nut_phai');

// //  hiệu ứng khi click vào nút phải
// nut_phai.addEventListener('click',click_nutPhai);
// function click_nutPhai() {
//     var child=slide.children[0];
//         slide.removeChild(child);
//         slide.appendChild(child);
// };

// //  hiệu ứng khi click vào nút trái
// nut_trai.addEventListener('click',click_nutTrai);
// function click_nutTrai(){
//         var child=slide.children[3];
//         var next_child=slide.children[0];
//             slide.removeChild(child);
//             slide.insertBefore(child,next_child);
// }


// phần video
var video = document.getElementById("video");

function play_pause() {
    if (video.paused == true) {
        video.play();
    }
    else {
        video.pause();
    }
}
function makeSmall() {
    video.width = 300;
}
function makeNomal() {
    video.width = 500;
}
function makeBig() {
    video.width = 830;
}
// phần bắt sự kiện scroll
var header = document.querySelector('header');

window.addEventListener("scroll", function () {
    if (window.pageYOffset) {
        if (window.pageYOffset >= 170) {
            header.classList.add('ps_fixed');

        }
        if (window.pageYOffset < 100) {
            header.classList.remove('ps_fixed')
        }
    }
})