var win = $(window),
    nav_on = null;
$(function () {
    // banner 切换
    (function () {
        var banner = $('#banner'),
            pic_c = banner.find('.pics'),//ul
            pics = pic_c.children(),//ul->li[]
            idx_c = banner.find('.idxs'),//ul
            idxs = idx_c.children(),//ul->li[]
            btns = banner.find('.btns a'),//a[]
            prev = btns.filter('.prev'),
            next = btns.filter('.next'),

            len = pics.length,
            idx = 0,
            prev_i = -1,
            max_i = len - 1,
            curr_p = pics.eq(idx),//equal pics.get(index),current picture
            curr_i = idxs.eq(idx),//current li-index of picture
            delay = 5000,
            timeout = -1;

        win.on('load', function () {
            idx_recu(0, 1500 / len, function () {
                setTimeout(function () {
                    curr_i.addClass('on');
                    auto();
                }, 300);
                idxs.hover(hover);
            });
            banner.hover(function () {
                // prev.stop().fadeIn(300);
                // next.stop().fadeIn(300);
                btns.addClass('on');
            }, function () {
                btns.removeClass('on');
                // prev.stop().fadeOut(300);
                // next.stop().fadeOut(300);
            });
            prev.on('click', function () { fade(idx === 0 ? idx = max_i : --idx) });
            next.on('click', function () { fade(idx === max_i ? idx = 0 : ++idx) });
        });

        function fade(idx) {
            clearTimeout(timeout);
            prev_i = idx;
            curr_p.stop(false, true).fadeOut(300);
            curr_p = pics.eq(idx).stop(false, true).fadeIn(300);
            curr_i.removeClass('on');
            curr_i = idxs.eq(idx).addClass('on');
            auto();
        }
        function hover() {
            idx = $(this).index();
            if (idx === prev_i) return;
            fade(idx);
        }
        function idx_recu(idx, delay, func) {
            temp = idxs.eq(idx);
            if (temp.length) {
                temp.css('margin-top', 0).fadeIn(500);
                setTimeout(function () {
                    idx_recu(idx + 1, delay, func);
                }, delay);
            } else {
                func();
                return;
            }
        }
        function auto() {
            timeout = setTimeout(function () {
                fade(idx === max_i ? idx = 0 : ++idx);
            }, delay);
        }
    }());

    // 新闻轮播
    (function () {
        var sup = $('#news-slide'),
            items = sup.children(),
            first = items.eq(0);

        win.on('load', function () { auto(); });

        function slide(elem) {
            elem = first;
            first.animate({ 'margin-top': -57 }, 500, function () {
                first = first.next();
                sup.append(elem.css('margin-top', 0));
            });
            auto();
        }

        function auto() {
            setTimeout(function () {
                slide();
            }, 5000);
        }
    }());

    // 滑块控制
    (function () {
        var sup = document.getElementById('subjects').getElementsByTagName('div')[0],//div
            items = sup.getElementsByTagName('a'),//a[]
            len = items.length;//4
        // base  = 70,
        // width_s = parseInt(getComputedStyle(sup).width);
        // width_i = width_s / len,
        // left    = null,
        // style   = null,
        // lefts = [],  // 普通模式下各元素的位置
        // styles  = [];

        win.on('load', function () { sup.className = "g-wrap ready state-0"; })

        // 存储常用变量, 绑定事件
        for (var i = 0, elem = null; i < len; i++) {
            elem = items[i];
            // styles.push(elem.style);
            // lefts.push(width_i*i);
            elem.setAttribute('idx', i + 1);//idx=1
            bind(elem, 'mouseover', hoverOn);//onmouseover=function(){};
        }
        // 元素逐个出现
        initItems(0, len, 200);

        // 事件绑定函数
        bind(sup, 'mouseout', function () {
            // for (i=0; i<len; i++) {
            // styles[i].left = lefts[i] + 'px';
            // }
            sup.className = "g-wrap state-0";
        });

        // 控制元素逐个出现
        function initItems(idx, len, delay) {
            if (idx === len) return;
            // style = styles[idx];
            // style.left = lefts[idx] + "px";
            // style.opacity = "1";
            setTimeout(function () { initItems(idx + 1, len, delay); }, delay);
        }

        // 函数绑定
        function bind(elem, evn, func) {
            if ('addEventListener' in elem) {
                bind = function (elem, evn, func) { elem.addEventListener(evn, func, false); }
            } else {
                bind = function (elem, evn, func) { elem['on' + evn] = func; }
            }
            bind(elem, evn, func);
        }

        //元素 mouseover 事件处理
        var i = 0, flag = false;
        function hoverOn() {
            // for(i=0, flag=false; i<len; i++) {
            //     temp = items[i];
            //     style = styles[i];
            //     if (temp === this) {
            //         flag = true;
            //         style.left = lefts[i] - (i*base) + "px";
            //     } else {
            //         style.left = lefts[i] + (flag?(len-i)*base:-(i*base)) + "px";
            //     }
            // }
            sup.className = "g-wrap state-" + this.getAttribute('idx');
        }
    }());


    (function () {
        var copyright = $('#copyright'),
            hei = $('html').height() - $('body').height();
        if (hei <= 0) return;
        copyright.find('.g-wrap div').height(37 + hei);
    }());
});