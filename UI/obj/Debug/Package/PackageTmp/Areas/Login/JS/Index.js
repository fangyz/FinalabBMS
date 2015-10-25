$(function () {
    $.ajax({
        url: '/Login/Login/GetMenuData',
        type: 'get',
        success: function (jsonMenu) {
            ResizeWindow();//根据浏览器的高度改变nav和main的高度
            InitLeftMenu(jsonMenu);
        }
    })
})

//窗体resize事件响应
$(function () {
ChangeFrame('00');
$("#left-shrink-btn").click(function () {
    LeftMenuShrink();        
});
//Onload事件响应
$(window).load(
    function () {
        ResizeWindow();
        InitLeftMenu();
    }
    );
//窗体resize事件响应
$(window).resize(
    function () {
        ResizeWindow();
    }
    );
});


//初始化左侧导航面板
function InitLeftMenu(jsonMenu) {
    var jsonMenus = jQuery.parseJSON(jsonMenu);//将json字符串转为json对象
    var $menu = $("#menu");
    $menu.empty();//删除选中元素所有子节点
    var menuList = "";
    menuList += '<ul>';
    var bgx = 0;//父列表的高度比子列表的小20px
    console.info(jsonMenus.menus);

    $.each(jsonMenus.menus, function (i, n) {
        console.info(n.menuId);
        console.info(n.icon.imgPath);
        console.info(n.menuName);
        menuList += '<li>';
        menuList += '<div  class="menu_column_title" onclick="SlideToggleChildMenu(this,' + n.menuId + ')"  id="menu-column-title-' + n.menuId + '">';
        menuList += '<div class="menu_column_title_border" >';
        menuList += '<div id="menu-column-title-logo-' + n.menuId + '" class="menu_column_title_logo" background-position:' + bgx + 'px ' + 0 + 'px;"></div>';
        menuList += '<span class="menu_column_title_text">' + n.menuName + '</span>';
        menuList += '<div id="menu-column-title-state-' + n.menuId + '" class="menu_column_title_state" style="background-image:url(\'Images/index_icon.png\');background-position-x:-240px;background-position-y:0px;"></div>';
        menuList += '</div>';
        menuList += '</div>';

        menuList += '<div class="child_menu" id="child-menu-' + n.menuId + '">';
        menuList += '<ul>';
        $.each(n.childMenus, function (j, o) {
            menuList += '<li  onclick="AddIframe(' + o.menuId + ',\'' + o.url + '\',\'' + o.menuName + '\')">';
            menuList += '<div class="child_menu_column_border">';
            menuList += '<div class="child_menu_column_logo"><span id="imgChild">O</span></div>';
            menuList += '<span class="child_menu_column_text" >' + o.menuName + '</span>';
            menuList += '</div>';
            menuList += '</li>';

        });
        menuList += '</ul>';
        menuList += '</div>';
        menuList += '</li>';
        bgx = bgx - 20;
    });

    menuList += "</ul>"
    $menu.append(menuList);
}



//根据浏览器高度改变nav和main的高度
function ResizeWindow()
{
    var windowHeight = $(window).outerHeight(); //获取浏览器高度
    var windowWidth = $(window).outerWidth(); //获取浏览器宽度
    var headerHeight = $("#header").outerHeight();//获取header高度
    var footerHeight = $("#footer").outerHeight();//获取footer高度
    //设置nav和main的高度
    $("#left").css('height', windowHeight - headerHeight-2+ 'px');
    $("#main").css('height', windowHeight - headerHeight-2 + 'px');//左边和右边高度是一样的
    $("#main").css('width', windowWidth - ($("#left").outerWidth()-2) + 'px');
    $("#main-panel").css('height', $("#main").outerHeight() - $('#top-nav').outerHeight() + 'px');
    $("#header-right").css('width', $("#header").outerWidth() - 187 + 'px');
    $("#footer").css('top', (windowHeight + 1) + 'px');

    ResizeChildMenu();//根据浏览器高度改变子列表的高度
    if ($(".menu").css("width") == '0px')//设置nav上的宽度，她是在0.1秒内增加到所要的宽度的
    {
        $("#nav-body").animate({ width: $(window).outerWidth() - 0 }, 100);
        $("#nav-body-ul").animate({ width: ($(window).outerWidth() - 0) }, 100);
    } else
    {
        $("#nav-body").animate({ width: $(window).outerWidth() - 185 },100);
        $("#nav-body-ul").animate({ width: $(window).outerWidth() - 198  }, 100);
    }
}
//根据浏览器高度改变子列表的高度，也就是左边人事管理大标签的子列表的高度
function ResizeChildMenu()
{
    var $childMenu = $(".child_menu");
    var $menuColumn = $(".menu_column_title");
    //$childMenu.css('height', $("#left").outerHeight() - $menuColumn.outerHeight() * $menuColumn.length + 'px');
}

//Show子菜单事件
function SlideToggleChildMenu(childMenu,id)
{
    var $childMenus = $(".child_menu");
    var $childMenu = $(childMenu);
    var $states = $(".menu_column_title_state");
    var $state = $("#menu-column-title-state-" + id);
    if ($state.css('background-position') == '-240px 0px')
    {
        setTimeout(function () { $state.css('background-position-x', '-260px'); }, 200); 
    } else
    {
        setTimeout(function () { $state.css('background-position-x', '-240px'); }, 200);
    }
    $childMenus.each(function (i, n) {
        if (n.style.display == "block" && $childMenu.next().css("display") != "block")
        {
            setTimeout(function () { $($states[i]).css('background-position-x', '-240px'); }, 100);
            $(n).slideToggle(500);
        }
    })
    ResizeChildMenu();

    var childNext=$childMenu.next();
    childNext.slideToggle(500);
}

//菜单滑动事件
function LeftMenuShrink()
{
    if ($(".menu").css("width") == '0px')
    {
        $("#left").animate({ width: 185 }, 500);
        $(".menu").animate({ width: 185 }, 500);
        $("#left-shrink-btn").animate({ left: 185 }, 500);
        setTimeout(function () { $("#left-shrink-btn").css('background-position-x', '0px'); }, 200);
        $("#main").animate({ left: 185, width: $(window).outerWidth() - 185 }, 500);
        $("#nav-body").animate({ width: $(window).outerWidth() - 185 }, 500);
        $("#nav-body-ul").animate({ width: $(window).outerWidth() - 185 }, 500);
    } else
    {
        $("#left").animate({ width: 0 }, 500);
        $(".menu").animate({ width: 0 }, 500);
        $("#left-shrink-btn").animate({ left: 0 }, 500);
        setTimeout(function () { $("#left-shrink-btn").css('background-position-x', '-20px'); }, 200);
        $("#main").animate({ left: 0, width: $(window).outerWidth() - 0 }, 500);
        $("#nav-body").animate({ width: $(window).outerWidth() - 0}, 500);
        $("#nav-body-ul").animate({ width: $(window).outerWidth() - 0 }, 500);
    }
}

//每当子列表点击时，触发这个事件
function AddIframe(id, url, name) {
    if (($("#nav-body-ul li").length + 1) * 100  >= $("#nav-body-ul").outerWidth()) {
        alert("您打开的页面太多惹~~关掉几个再试试？？");
        return;
    }
    var strLi = "";
    var strIframe = "";

    var $ul = $("#nav-body ul");

    var $mainPanel = $("#main-panel");

    var $mainPanelChilds = $mainPanel.children();

    $mainPanelChilds.each(function (i, n) {
        n.style.display = "none";
    });
    
    if (document.getElementById("nav-column-" + id)) {
        document.getElementById("panel-body-" + id).style.display = 'block';
    } else
    {
        
        strLi += '<li id="nav-column-' + id + '">';
        strLi += '<div class="main_nav_li_interlayer" onclick="ChangeFrame(' + id + ')" >'
        strLi += '<div class="mian_nav_li_content">';
        strLi += '<span class="mian_nav_li_content_title">' + name + '</span>';
        strLi += '</div>';
        strLi += '</div>'
        strLi += '<div onclick="SubIframe('+id+')" onmouseover ="NavCloseBtnMouseOver(' + id + ')" onmouseout ="NavCloseBtnMouseOut(' + id + ')" class="mian_nav_li_close_btn" id="nav-li-close-btn-' + id + '"></div>';
        strLi += '</li>';

        $ul.append(strLi);

        strIframe += '<div id="panel-body-'+id+'" style="width:inherit;height:inherit">';
        strIframe += '<iframe src="'+url+'" style="border:0px none;width:100%;height:100%;">';
        strIframe += '</iframe>';
        strIframe += '</div>';

        $mainPanel.append(strIframe);

    }

    $("#nav-body-ul").children().each(function (i, n) {
        $(n).css("background-position-y", "-20px");
    });

    $("#nav-column-" + id).css("background-position-y", "-100px");

}

function NavCloseBtnMouseOver(navId)
{
    $li = $("#nav-column-" + navId);
    var oldBPY = $li.css('background-position-y');
    $li.css('background-position-y',oldBPY.substring(0,oldBPY.length-2) - 40 +"px");
}

function NavCloseBtnMouseOut(navId) {
    $li = $("#nav-column-" + navId);
    var oldBPY = $li.css('background-position-y');
    $li.css('background-position-y',Number(oldBPY.substring(0,oldBPY.length-2)) + 40 +"px");
}

function SubIframe(id)
{
    var $navLi = $("#nav-column-" + id);
    $navLi.remove();
    var $iframe = $("#panel-body-" + id);
    var $nextIframe = $iframe.next();

    //若$nextIframe不存在length = 0 ，否则为length = 1
    if ($nextIframe.length <= 0) {
        $nextIframe = $iframe.prev();
        if ($nextIframe.length <= 0) {
            $nextIframe = $iframe;
        }
    }

    if ($iframe.css("display") == "block") {
        ChangeFrame($nextIframe.attr('id').substring(11, 13));//截取id号
    }
    $iframe.remove();
}
var count = 0;
/*iframe   title onclick事件*/
function ChangeFrame(id)
{
    var $mainPanel = $("#main-panel");
    var $mainPanelChilds = $mainPanel.children();
    $mainPanelChilds.each(function (i, n) {
        n.style.display = "none";
    });
        document.getElementById("panel-body-" + id).style.display = 'block';

    $("#nav-body-ul").children().each(function (i, n) {
        $(n).css("background-position-y", "-20px");
    });

    $("#nav-column-" + id).css("background-position-y","-100px");
}
//关闭所有左边面板
function CloseLeft() {
    var ul = document.getElementById("nav-body-ul");
    var li = ul.childNodes;
    var count = li.length;
    for (var i = count-1; i>=0; i--) {
        ul.removeChild(li[i]);
    }
    var panel = document.getElementById("main-panel");
    var pbody = panel.childNodes;
    var one = document.getElementById("panel-body-00");
    count = pbody.length;
    for (var i = count - 1; i >= 0; i--) {
        if (one == pbody[i]) {
            one.style.display = "";
        } else {
            panel.removeChild(pbody[i]);
        }
       
    }
    

}

function showClose() {
    var close = document.getElementById("leftClose");
    close.style.fontSize = "18px";
    close.style.color = "#003466";
}
function oldClose() {
    var close = document.getElementById("leftClose");
    close.style.fontSize = "12px";
    close.style.color = "#67a3a1";
}