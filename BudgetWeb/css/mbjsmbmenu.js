function IsAllDefined() { for (var a = 0; a < arguments.length; ++a) { if (typeof (arguments[a]) == "undefined") { return false } } return true } function asNum(a) { return parseInt(a, 10) || 0 } function getStyle(b, a, d) { var c = ""; if (b.currentStyle) { c = b.currentStyle[d] } else { if (window.getComputedStyle) { c = document.defaultView.getComputedStyle(b, null).getPropertyValue(a) } } return c } function GetWinH() { var b = 0; var a = 0; if ((!document.compatMode || document.compatMode == "CSS1Compat") && document.documentElement) { a = document.documentElement } else { if (document.body) { a = document.body } } if (a && a.clientHeight) { b = a.clientHeight } else { if (IsAllDefined(window.innerWidth, window.innerHeight, document.width)) { b = window.innerHeight; if (document.width > window.innerWidth) { b = b - 15 } } } return b } function GetWinW() {
    var a = 0; var b = 0; if ((!document.compatMode || document.compatMode == "CSS1Compat") && document.documentElement) { b = document.documentElement } else { if (document.body) { b = document.body } } if (b && b.clientWidth) { a = b.clientWidth } else {
        if (IsAllDefined(window.innerWidth, window.innerHeight, document.height)) {
            a = window.innerWidth;
            if (document.height > window.innerHeight) { a = a - 15 }
        }
    } return a
} function GetObjectRect(f) { var a = 0; var i = 0; var g = f; while (f && f != null) { a += asNum(isNS4 ? f.pageX : f.offsetLeft); i += asNum(isNS4 ? f.pageY : f.offsetTop); if (isIE) { if (f.offsetParent && f.offsetParent.style && f.offsetParent.style.position == "absolute") { var c = asNum(getStyle(f.offsetParent, "border-left-width", "borderLeftWidth")); var d = asNum(getStyle(f.offsetParent, "border-top-width", "borderTeftWidth")); a -= c; i -= d } } if (isNS4) { if (f.style && (f.style.position == "absolute" || f.style.position == "relative")) { break } } f = f.offsetParent } f = g; var b = 0; var e = 0; if (isOp && !isOp7) { b = f.style.pixelWidth } else { if (isNS4) { b = f.clip.width } else { b = f.offsetWidth } } if (isOp && !isOp7) { e = f.style.pixelHeight } else { if (isNS4) { e = f.clip.height } else { e = f.offsetHeight } } return { x: a, y: i, w: b, h: e } } function LoadSrcImage(b) { var a = new Image(); if (typeof isIE == "undefined") { GetBrowserInfo() } if (isIE && window.location.protocol == "file:") { a.meSrc = b } else { a.src = b } return a } function GetBrowserInfo() {
    isDOM = document.getElementById;
    isMz = isDOM && (navigator.appName == "Netscape"); isOp = isDOM && window.opera; isIE = document.all && document.all.item && !isOp; isNS4 = document.layers; isOp7 = isOp && document.readyState
} function GetViewRect() { var c = 0; var a = 0; if (isNS4 || isMz || isOp) { a = window.pageXOffset; c = window.pageYOffset } else { var b = (document.compatMode == "CSS1Compat" && !isMz) ? document.documentElement : document.body; a = b.scrollLeft; c = b.scrollTop } return { x: a, y: c, w: GetWinW(), h: GetWinH() } } function SetElemOpacity(a, b) { if (a && a.style) { if (b == 1) { a.style.opacity = ((navigator.userAgent.indexOf("Gecko") > -1) && !/Konqueror|Safari|KHTML/.test(navigator.userAgent)) ? 0.999999 : null; if (isIE) { if (a.style.filter) { a.style.filter = a.style.filter.replace(/alpha\([^\)]*\)/gi, "") } } } else { if (b < 0.00001) { b = 0 } a.style.opacity = b; if (isIE) { a.style.filter = (a.style.filter ? a.style.filter.replace(/alpha\([^\)]*\)/gi, "") : "") + "alpha(opacity=" + b * 100 + ")" } } } } function SetElementScale(d, m, l) {
    if (d && d.style) {
        try {
            if (m > 1) { m = 1 } if (l > 1) { l = 1 } if (isIE) {
                if (m == 1 && l == 1) {
                    d.style.filter = d.style.filter.replace(/progid:DXImageTransform.Microsoft.Matrix\([^\)]*\);?/gi, "")
                } else { var c = 0; var b = 0; if (d.cbnDirectionX == -1 && d.cbnMenuRect) { c = d.cbnMenuRect.w - d.cbnMenuRect.w * m } if (d.cbnDirectionY == -1 && d.cbnMenuRect) { b = d.cbnMenuRect.h - d.cbnMenuRect.h * l } m = Math.round(m * 100) / 100; l = Math.round(l * 100) / 100; d.style.filter = (d.style.filter ? d.style.filter.replace(/progid\:DXImageTransform\.Microsoft\.Matrix\([^\)]*\)/gi, "") : "") + "progid:DXImageTransform.Microsoft.Matrix(sizingMethod='clip to original', M11=" + m + ", M12=0, M21=0, M22=" + l + " Dx=" + c + " Dy=" + b + ")" }
            } else { var k = "0"; var h = "0"; if (d.cbnDirectionX == -1) { k = "100%" } if (d.cbnDirectionY == -1) { h = "100%" } if (d.ebnStyleScaleProperty) { a = d.ebnStyleScaleProperty } else { var a = null; var j = ["transform", "MozTransform", "WebkitTransform", "OTransform"]; for (var f = 0; f < j.length; f++) { if (d.style[j[f]] != null) { a = j[f]; break } } d.ebnStyleScaleProperty = a } if (a != null) { if (m == 1 && l == 1) { d.style[a] = "" } else { d.style[a] = "scale(" + m + "," + l + ")"; d.style[a + "Origin"] = k + " " + h } } }
        } catch (g) { }
    }
} function ebmResetElementScale(a) { SetElementScale(a, 1, 1) } function ebmProgressElementScale(b, a) {
    if (b.ebmScaleEffect == 1) {
        SetElementScale(b, 1, a)
    } else { if (b.ebmScaleEffect == 2) { SetElementScale(b, a, a) } else { if (b.ebmScaleEffect == 3) { SetElementScale(b, a, 1) } } }
} function ebmSetDivShadow(e, b, a) { var f = null; var d = ["boxShadow", "MozBoxShadow", "WebkitBoxShadow"]; for (var c = 0; c < d.length; c++) { if (e.style[d[c]] != null) { f = d[c]; break } } if (f == null) { if (isIE) { e.style.filter += "progid:DXImageTransform.Microsoft.DropShadow(OffX=" + b + ", OffY=" + a + ", Color='#80777777', Positive='true')"; return true } else { return false } } e.style[f] = b + "px " + a + "px " + ((b + a) / 2) + "px #777777"; return true } function ebmGetBorderPadding(a) { var b = asNum(getStyle(a, "border-top-width", "borderTopWidth")); b = b + asNum(getStyle(a, "padding-top", "paddingTop")); return b } function ebmCreateMenuDiv(d, b, a) {
    var c = document.createElement("div"); c.id = d; c.className = b; c.style.position = "absolute"; c.style.left = "0px"; c.style.top = "0px"; c.ebmFadeEffect = ebmFadeEffect; if (ebmFadeEffect) { SetElemOpacity(c, 0) } else { if (cbnMenuAlpha) { SetElemOpacity(c, cbnMenuAlpha) } } c.ebmScaleEffect = ebmScaleEffect; if (a && ebmScaleEffect > 0) {
        ebmResetElementScale(c)
    } c.cbnMenuAlpha = cbnMenuAlpha; c.cbnBorderPadding = ebmGetBorderPadding(c); c.cbnDefaultDirectionX = cbnDefaultDirectionX; return c
} function ebmTickerOn(b) { for (var a = b; a; a = a.openSubmenuDiv) { if (!a.ticker && a.id) { a.ticker = setTimeout('ebmRemoveSubmenu("' + a.id + '");', 550) } } } function ebmTickerOff(b) { for (var a = b; a; a = a.upperTR ? a.upperTR.menuDiv : 0) { if (a.ticker) { clearTimeout(a.ticker); a.ticker = 0 } } } function ebmMenuPosY(b, j, i, d, h, g, e) { var a = 5; var f = i; var k = h; var c = g; if (k > j - 2 * a && j > 0) { f = a + b; k = j - 2 * a } else { if (c == -1) { f = i + d - k + e } else { f = i } if (f < b + a) { f = b + a; c = 1 } if (f + h > j + b - a && j > 0) { f -= f + k - (j + b - a); c = -1 } } return { y: f, direction: c, size: k } } function ebmMenuPosX(c, d, i, k, b, h, f) { var a = 5; var g = i; var j = b; var e = h; if (((e >= 0) && (i + k + b > d + c - a)) || ((e < 0) && (i - b < a))) { if (i - c > d + c - (i + k) && d > 0) { e = -1 } else { e = 1 } } if (e >= 0) { g = i + k; if (d + c - a - g < j && d > 0) { j = d + c - a - g } } else { g = i - j + f; if (g - c < a) { g = c + a; j = i - (c + a) } } return { x: g, direction: e, size: j } } function ebmFade(f) {
    var b = document.getElementById(f); if (b) {
        var a = new Date().getTime(); var d = a - b.cbnLastAnimTime; var c = d / 200; if (c < 0.05 || b.cbnTransitionProgress == 0) {
            c = 0.05
        } b.cbnTransitionProgress = c; if (b.cbnTransitionProgress > 1) { b.cbnTransitionProgress = 1 } if (b.ebmFadeEffect) { var e = b.cbnTransitionProgress; if (b.cbnMenuAlpha && e > b.cbnMenuAlpha) { e = b.cbnMenuAlpha } SetElemOpacity(b, e) } if (b.ebmScaleEffect > 0) { ebmProgressElementScale(b, b.cbnTransitionProgress) } if (b.cbnTransitionProgress >= 1) { clearInterval(b.ebmFadeTimer); b.ebmFadeTimer = null }
    }
} function ebmDisplaySubmenu(i, d, j) {
    var f = document.getElementById(i); if (f && f.style) {
        if (f.style.visibility == "visible") { ebmTickerOff(f); return } f.style.left = "0px"; f.style.top = "0px"; f.style.height = "auto"; f.style.width = "auto"; if (!f.depth && (cbnOpenTopMenu != f)) { ebmRemoveSubmenu(cbnOpenTopMenu.id) } if (d && d.menuDiv && d.menuDiv.openSubmenuDiv && d.menuDiv.openSubmenuDiv != f) { ebmRemoveSubmenu(d.menuDiv.openSubmenuDiv.id) } ebmTickerOff(f); if (f.depth > 0) { f.cbnDirectionX = f.upperTR.menuDiv.cbnDirectionX; f.cbnDirectionY = f.upperTR.menuDiv.cbnDirectionY } else { f.cbnDirectionX = f.cbnDefaultDirectionX; f.cbnDirectionY = 1 } f.style.overflow = "visible"; var c = d; if (c.tagName && c.tagName.toLowerCase() == "a") {
            c = c.parentNode
        } var a = GetObjectRect(c); var k = GetObjectRect(f); var g = GetViewRect(); var e; if (j) { e = ebmMenuPosY(g.y, g.h, a.y, a.h, k.h, f.cbnDirectionY, 0) } else { e = ebmMenuPosX(g.y, g.h, a.y, a.h, k.h, f.cbnDirectionY, 0); e.y = e.x } f.cbnDirectionY = e.direction; if (e.size < k.h && e.size > 0) { if (isOp && !f.OrigWidth) { f.OrigWidth = f.clientWidth } f.style.overflow = "auto"; if (isIE) { f.style.width = (f.offsetWidth + 18) + "px"; f.style.overflowX = "visible" } else { if (isMz) { f.style.marginRight = 20 } } f.style.height = e.size + "px"; f.scrollTop = 0; f.scrollLeft = 0; if (isOp) { f.style.width = f.OrigWidth + "px" } } f.style.top = e.y - k.y + "px"; k = GetObjectRect(f); var h; if (f.depth) { if (f.cbnBorderPadding == 0) { f.cbnBorderPadding = ebmGetBorderPadding(f) } h = f.cbnBorderPadding } else { h = 0 } if (j) { e = ebmMenuPosX(g.x, g.w, a.x, a.w, k.w, f.cbnDirectionX, h) } else { e = ebmMenuPosY(g.x, g.w, a.x, a.w, k.w, f.cbnDirectionX, h); e.x = e.y } f.cbnDirectionX = e.direction; if ((e.size < k.w) && (f.cbnDirectionX > 0)) { e.x = e.x - (k.w - e.size) } f.style.left = e.x - k.x + "px"; f.cbnMenuRect = { w: k.w, h: k.h, x: e.x - k.x, y: e.y - k.y }; if (f.ebmFadeEffect || ebmScaleEffect > 0) {
            if (!f.ebmFadeTimer) {
                if (f.ebmFadeEffect) {
                    SetElemOpacity(f, 0.05)
                } if (f.ebmScaleEffect > 0) { ebmProgressElementScale(f, 0.05) } var b = 'ebmFade("' + i + '");'; f.cbnTransitionProgress = 0; f.cbnLastAnimTime = new Date().getTime(); f.ebmFadeTimer = setInterval(b, 20)
            }
        } if (!f.depth) { cbnOpenTopMenu = f } else { d.menuDiv.openSubmenuDiv = f; d.MakeExpanded() } f.style.visibility = "visible"
    }
} function ebmRemoveSubmenu(b) { var a = document.getElementById(b); if (a && (a.style.visibility == "visible")) { if (a.openSubmenuDiv) { ebmRemoveSubmenu(a.openSubmenuDiv.id) } a.style.visibility = "hidden"; a.openSubmenuDiv = 0; a.RemoveSelection(); if (a.upperTR) { a.upperTR.MakeNormal() } if (a.ticker) { clearTimeout(a.ticker); a.ticker = null } if (a.ebmFadeEffect || a.ebmScaleEffect) { if (a.ebmFadeEffect) { SetElemOpacity(a, 0) } if (ebmScaleEffect > 0) { ebmResetElementScale(a) } if (a.ebmFadeTimer) { clearTimeout(a.ebmFadeTimer); a.ebmFadeTimer = null } } } } function ebmGenerateTree(a, h, w, b) {
    var s = document.getElementById("BtnMenuContainer" + ebmMenuName); var v = ebmCreateMenuDiv(a.id + "mdiv", b, true); s.appendChild(v); if (useShadow) {
        ebmSetDivShadow(v, ShadowOffsetX, ShadowOffsetY)
    } v.upperTR = h; v.depth = w; v.openSubmenuDiv = 0; v.style.zIndex = 102 + v.depth * 3; v.RemoveSelection = function () { if (this.childNodes[0].rows) { for (var k = 0; k < this.childNodes[0].rows.length; k++) { var j = this.childNodes[0].rows[k]; if (j.tagName && j.tagName.toLowerCase() == "tr") { j.className = j.className.replace("hot", "") } } } }; v.onmouseover = function () { meDoMouseOver(this) }; v.onmouseout = function () { meDoMouseOut(this) }; var f = document.createElement("table"); v.appendChild(f); f.cellSpacing = 0; var n = /^([a-zA-Z]*?\:\/\/)?[^\(\)\:]*?(\?.*)?$/; for (var r = 0; r < a.childNodes.length; r++) {
        var u = a.childNodes[r]; if (u.tagName && u.tagName.toLowerCase() == "li") {
            var q = f.insertRow(-1); q.menuDiv = v; q.MakeExpanded = function () { this.className = this.className + " expanded" }; q.MakeNormal = function () { this.className = this.className.replace("expanded", "") }; q.className = u.className; var d = null; var i = null; var c = null; var e = null; for (var p = 0; p < u.childNodes.length; p++) {
                var o = u.childNodes[p]; if (o.tagName && o.tagName.toLowerCase() == "a") { c = o } else {
                    if (o.tagName && o.tagName.toLowerCase() == "span" && o.className && o.className.substr(0, 8) == "ebul_img") {
                        if (!c) {
                            if (!i) {
                                i = o
                            }
                        }
                    } else { if (o.tagName && o.tagName.toLowerCase() == "img") { if (!c) { if (!d) { d = o } } } else { if (o.tagName && o.tagName.toLowerCase() == "ul") { e = o } } }
                }
            } if (c != null || d != null || i != null || e != null) {
                var t = q.insertCell(-1); t.style.borderRightWidth = "0px"; t.style.paddingRight = "2px"; if (d) { t.appendChild(d) } else { if (i) { t.appendChild(i) } else { t.innerHTML = "&nbsp;" } } var m = q.insertCell(-1); m.style.borderRightWidth = "0px"; m.style.borderLeftWidth = "0px"; m.style.paddingRight = "4px"; m.style.paddingLeft = "4px"; if (c) { m.appendChild(c); if (c.href && c.href.match(n) && !c.target) { q.rowClickLink = c.href; q.onclick = function () { window.location.href = this.rowClickLink; return false } } } else { m.innerHTML = "&nbsp;" } var g = q.insertCell(-1); g.style.borderLeftWidth = "0px"; g.style.paddingLeft = "4px"; if (e) { if (markerSymbol) { g.innerHTML = '<a style="text-decoration: none;">' + markerSymbol + "</a>" } else { g.innerHTML = "&nbsp;" } q.cbnTRSubmenuId = ebmGenerateTree(e, q, w + 1, b) } else { g.innerHTML = "&nbsp;" } q.onmouseover = function () {
                    this.menuDiv.RemoveSelection(); this.className = this.className + " hot";
                    if (this.cbnTRSubmenuId) { ebmDisplaySubmenu(this.cbnTRSubmenuId, this, 1) } else { if (this.menuDiv.openSubmenuDiv) { ebmTickerOn(this.menuDiv.openSubmenuDiv) } }
                }; q.onmouseout = function () { this.menuDiv.RemoveSelection() }
            } else { var x = q.insertCell(-1); var l = document.createElement("div"); l.className = b + "separator"; x.className = b + "separatortd"; x.colSpan = 3; x.appendChild(l) }
        }
    } return v.id
} function meDoMs(b) { var a = b.substring(0, b.length - 1); var c = window["mbi_" + b].src; if (isIE) { if (window.location.protocol != "file:") { c += "#" } else { c = window["mbi_" + b].meSrc } } if (document["mbi_" + a]) { document["mbi_" + a].src = c } return false } function meDoShow(e, b, c) { var d = "ebul_" + e + "mdiv"; var a = document.getElementById(d); if (a && a.style) { ebmTickerOff(cbnOpenTopMenu); ebmDisplaySubmenu(d, c, b) } } function meDoMouseOut(a) { if (a) { ebmTickerOn(cbnOpenTopMenu) } } function meDoMouseOver(a) { if (a) { ebmTickerOff(a) } } function InitEasyMenu() {
    GetBrowserInfo(); if (isMz && !cbnMenuAlpha && !ebmFadeEffect) { cbnMenuAlpha = 1 } var d = document.getElementsByTagName("img"); for (var c = 0;
    c < d.length; c++) { if (d[c].id && d[c].id.substring(0, 4) == "mbi_" && d[c].parentNode && d[c].parentNode.tagName && d[c].parentNode.tagName.toLowerCase() == "a") { var b = d[c].parentNode; var e = null; if (b.parentNode && b.parentNode.parentNode && b.parentNode.parentNode.parentNode && b.parentNode.parentNode.parentNode.parentNode) { e = b.parentNode.parentNode.parentNode.parentNode; if (!(e.tagName && e.tagName.toLowerCase() == "table")) { e = null } } if (!e && b.parentNode && b.parentNode.parentNode) { e = b.parentNode.parentNode; if (!(e.tagName && e.tagName.toLowerCase() == "ul")) { e = null } } if (e) { if (e.id == InitTable) { b.buttonnumber = d[c].id.substring(4); b.ebmMenuDirection = ebmMenuDirection; b.onmouseover = function () { meDoMs(this.buttonnumber + "o"); meDoShow(this.buttonnumber, this.ebmMenuDirection, this) }; b.onmouseout = function () { meDoMs(this.buttonnumber + "n"); meDoMouseOut(this) }; b.onmouseup = function () { meDoMs(this.buttonnumber + "o") }; b.onmousedown = function () { meDoMs(this.buttonnumber + "c") } } } } } document.write('<div id="BtnMenuContainer' + ebmMenuName + '"></div>');
    var a = document.getElementsByTagName("ul"); for (var c = 0; c < a.length; c++) { if (a[c].id && a[c].id.substring(0, 5) == "ebul_" && a[c].className.substring(0, 5) == "ebul_") { ebmGenerateTree(a[c], 0, 0, a[c].className) } }
}

var cbnOpenTopMenu = 0;
mbi_mbmenu_1n = LoadSrcImage('menu_files/ebbtmbmenu1.png');
mbi_mbmenu_1o = LoadSrcImage('menu_files/ebbtmbmenu1_1.png');
mbi_mbmenu_1c = LoadSrcImage('menu_files/ebbtmbmenu1_2.png');
mbi_mbmenu_2n = LoadSrcImage('menu_files/ebbtmbmenu2.png');
mbi_mbmenu_2o = LoadSrcImage('menu_files/ebbtmbmenu2_1.png');
mbi_mbmenu_2c = LoadSrcImage('menu_files/ebbtmbmenu2_2.png');
mbi_mbmenu_3n = LoadSrcImage('menu_files/ebbtmbmenu3.png');
mbi_mbmenu_3o = LoadSrcImage('menu_files/ebbtmbmenu3_1.png');
mbi_mbmenu_3c = LoadSrcImage('menu_files/ebbtmbmenu3_2.png');


var markerSymbol = "&raquo;";
var ShadowOffsetX = 2;
var ShadowOffsetY = 2;
var useShadow = true;
var InitTable = "mbmenuebul_table";
var cbnMenuAlpha = 0;
var ebmFadeEffect = false;
var ebmScaleEffect = 1;
var ebmMenuDirection = 0;
var ebmMenuName = "mbmenu";
var cbnDefaultDirectionX = 1; InitEasyMenu();