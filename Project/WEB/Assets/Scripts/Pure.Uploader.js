/**
 * HTML5 Pure Uploader 3.0
 * http://www.tqera.com/
 *
 * Copyright 2014, Lyzerk
 *
 * Date: 30-01-2015
 * License : http://codecanyon.net/item/pure-uploader/4452882
 */

function printLog(e) {
    console.log("------Log Report------"),
    console.log(new Date),
    console.log(e),
    console.log("------##########------")
}
function printError(e) {
    "object" == typeof e ? (e.message && (console.log("------Error Message------"),
    console.log("\nMessage: " + e.message)),
    e.stack && console.log(e.stack),
    console.log("------#############------")) : console.log("printError :: argument is not an object")
}
function contains(e, t) {
    for (var i in t)
        if (e.match(t[i]))
            return !0;
    return !1
}
function getFileExtension(e) {
    var t = e.split(".");
    return 1 === t.length || "" === t[0] && 2 === t.length ? "" : t.pop().toLowerCase()
}
function endsWith(e, t) {
    return -1 !== e.indexOf(t, e.length - t.length)
}
function humanFileSize(e, t) {
    var i = t ? 1e3 : 1024;
    if (i > e)
        return e + " B";
    var a = t ? ["KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"] : ["KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB"]
      , r = -1;
    do
        e /= i,
        ++r;
    while (e >= i);return e.toFixed(1) + " " + a[r]
}
function extend() {
    var e, t, i, a, r, n, o = arguments[0] || {}, s = 1, h = arguments.length, l = !1;
    for ("boolean" == typeof o && (l = o,
    o = arguments[1] || {},
    s = 2),
    "object" == typeof o || jQuery.isFunction(o) || (o = {}),
    h === s && (o = this,
    --s); h > s; s++)
        if (null != (e = arguments[s]))
            for (t in e)
                i = o[t],
                a = e[t],
                o !== a && (l && a && (jQuery.isPlainObject(a) || (r = jQuery.isArray(a))) ? (r ? (r = !1,
                n = i && jQuery.isArray(i) ? i : []) : n = i && jQuery.isPlainObject(i) ? i : {},
                o[t] = jQuery.extend(l, n, a)) : void 0 !== a && (o[t] = a));
    return o
}
function b64toBlob(e, t, i) {
    t = t || "",
    i = i || 512;
    for (var a = atob(e), r = [], n = 0; n < a.length; n += i) {
        for (var o = a.slice(n, n + i), s = new Array(o.length), h = 0; h < o.length; h++)
            s[h] = o.charCodeAt(h);
        var l = new Uint8Array(s);
        r.push(l)
    }
    var m = new Blob(r,{
        type: t
    });
    return m
}
function b64toBytes(e, t, i) {
    t = t || "",
    i = i || 512;
    for (var a = atob(e), r = [], n = 0; n < a.length; n += i) {
        for (var o = a.slice(n, n + i), s = new Array(o.length), h = 0; h < o.length; h++)
            s[h] = o.charCodeAt(h);
        var l = new Uint8Array(s);
        r.push(l)
    }
    return r
}
function isFunction(e) {
    var t = {};
    return e && "[object Function]" === t.toString.call(e)
}
function PureUploader(e) {
    function t(e) {
        e.watermark.image && (N.src = x.watermark.image,
        N.onerror = function() {
            printLog(e.errors.INIT_WATERMARK_IMAGE),
            e.watermark.image = ""
        }
        )
    }
    function i(e) {
        return e.drop ? e.dropPlace ? (e.dropPlace.ondragover = function() {
            return this.className.match(e.dragHoverClass) || (this.className += " " + e.dragHoverClass),
            !1
        }
        ,
        e.dropPlace.ondragleave = function() {
            return this.className.match(e.dragHoverClass) && (this.className = this.className.replace(e.dragHoverClass, "")),
            !1
        }
        ,
        e.dropPlace.ondrop = function(e) {
            return this.className.match(x.dragHoverClass) && (this.className = this.className.replace(x.dragHoverClass, "")),
            c(e.dataTransfer.files),
            !1
        }
        ,
        !0) : !1 : !0
    }
    function a(e) {
        return e.imageHolder ? !0 : !1
    }
    function r(e) {
        return e.file_input && "INPUT" == e.file_input.tagName && "file" == e.file_input.type ? (e.file_input.addEventListener("change", d, !1),
        !0) : !1
    }
    function n(e) {
        if (e.file_filter || " " != e.file_filter) {
            var t = e.file_filter.split("|");
            return t.length < 1 ? !1 : (L = t,
            !0)
        }
        return !1
    }
    function o(e) {
        return e.image.thumb ? e.image.thumb_width && e.image.thumb_height ? e.image.thumb_width > 10 && e.image.thumb_height > 10 ? !0 : (x.error(S, e.errors.THUMB_SIZE),
        !1) : !1 : !0
    }
    function s(e) {
        return parseInt(e.file_size) ? !0 : (x.error(S, e.errors.FILE_SIZE_INT),
        !1)
    }
    function h(e) {
        return e.form ? !0 : !1
    }
    function l(e) {
        return !e.image.thumb && !e.icon.icon || e.template.match(/{image}/g) ? !0 : !1
    }
    function m(e, t, i) {
        try {
            var a = document.createElement("div");
            a.id = e,
            a.className = x.file_class,
            a.appendChild(z);
            var r = x.template.replace(/{id}/g, e);
            r = r.replace(/{uploader}/g, x.name),
            r = x.image.thumb || x.icon.icon ? r.replace("{image}", "<img src='" + x.image.preparing + "' id=img_" + e + " />") : r.replace("{image}", ""),
            r = r.replace("{file.name}", t),
            r = r.replace("{file.size}", i),
            a.innerHTML += r,
            x.imageHolder.appendChild(a),
            x.imageHolder.appendChild(z)
        } catch (n) {
            x.error(S, x.errors.TEMPLATE_CREATE, n)
        }
    }
    function c(e) {
        for (var t in e)
            if ("object" == typeof e[t])
                if (contains(getFileExtension(e[t].name), L))
                    if (e[t].size < x.file_size)
                        if (0 == x.limit)
                            F.push(e[t]);
                        else if (R.length <= x.limit)
                            F.push(e[t]);
                        else {
                            var i = x.errors.UPLOAD_LIMIT.replace(/{limit}/g, x.limit);
                            x.error(S, i)
                        }
                    else
                        x.error(S, x.errors.FILE_SIZE_LARGER);
                else
                    x.error(S, x.errors.FILE_DOESNT_SUPPORT);
        g()
    }
    function d(e) {
        c(e.target.files),
        x.file_input.value = ""
    }
    function g() {
        x.auto_upload && v(null);
        var e = F.shift();
        if (null != e) {
            if (0 != x.limit && R.length + 1 > x.limit) {
                var t = x.errors.UPLOAD_LIMIT.replace(/{limit}/g, x.limit);
                return x.error(S, t),
                !1
            }
            var i = "temp_" + (new Date).getTime();
            m(i, e.name, humanFileSize(e.size, !0)),
            u(i, e)
        }
    }
    function u(e, t) {
        x.chunk.active ? w(e, t) : p(e, t)
    }
    function p(e, t) {
        var i = new FileReader
          , a = document.getElementById(e);
        i.onload = function(i) {
            if (i.preventDefault(),
            isCanvasText && t.type.match("image") && !t.type.match("gif")) {
                var r = document.createElement("canvas")
                  , n = r.getContext("2d")
                  , o = document.createElement("canvas")
                  , s = o.getContext("2d")
                  , h = new Image;
                h.onload = function() {
                    var e, i, l = 1;
                    if (e = x.image.resize_width && 0 != x.image.resize_width ? x.image.resize_width : h.width,
                    i = !x.image.keep_aspect_ratio && x.image.resize_height && 0 != x.image.resize_height ? x.image.resize_height : x.image.keep_aspect_ratio && x.image.resize_width && 0 != x.image.resize_width ? x.image.resize_width / (h.width / h.height) : h.height,
                    h.width > x.image.thumb_width ? l = x.image.thumb_width / h.width : h.height > x.image.thumb_height && (l = x.image.thumb_height / h.height),
                    o.width = e,
                    o.height = i,
                    s.drawImage(h, 0, 0, o.width, o.height),
                    x.watermark.watermark) {
                        var m = document.createElement("canvas")
                          , c = m.getContext("2d");
                        if (m.width = e,
                        m.height = i,
                        x.watermark.image) {
                            var d = N.width
                              , u = N.height;
                            x.watermark.image_aspect_ratio && (d = N.width,
                            u = N.height / (e / i));
                            var p = 0
                              , f = u;
                            x.watermark.position.match("bottom") ? f = m.height - u : x.watermark.position.match("top") ? f = 0 : x.watermark.position.match("center") && (f = (m.height - u) / 2),
                            x.watermark.position.match("left") ? p = 0 : x.watermark.position.match("mid") ? p = (m.width - d) / 2 : x.watermark.position.match("right") && (p = m.width - d),
                            c.globalAlpha = x.watermark.alpha,
                            c.drawImage(N, p, f, d, u)
                        } else if (x.watermark.text) {
                            c.fillStyle = x.watermark.color,
                            c.font = x.watermark.font_size + " " + x.watermark.font,
                            c.globalAlpha = x.watermark.alpha;
                            var w = c.measureText(x.watermark.text)
                              , _ = x.watermark.font_size.match("px") ? x.watermark.font_size.replace("px", "") : x.watermark.font_size
                              , p = 0
                              , f = _;
                            x.watermark.position.match("bottom") ? f = m.height - .3 * _ : x.watermark.position.match("top") ? p = 0 + .3 * _ : x.watermark.position.match("center") && (f = (m.height - _) / 2),
                            x.watermark.position.match("left") ? p = 0 : x.watermark.position.match("mid") ? p = (m.width - w.width) / 2 : x.watermark.position.match("right") && (p = m.width - w.width),
                            c.translate(p, f),
                            c.fillText(x.watermark.text, 0, 0)
                        }
                        s.drawImage(m, 0, 0, m.width, m.height, 0, 0, m.width, m.height)
                    }
                    if (r.width = h.width * l,
                    r.height = h.height * l,
                    n.drawImage(o, 0, 0, o.width, o.height, 0, 0, r.width, r.height),
                    R.push({
                        data: o.toDataURL(t.type),
                        name: t.name,
                        id: a.id,
                        state: "ready"
                    }),
                    x.image.thumb)
                        try {
                            r.style.cursor = "pointer",
                            r.onclick = function() {
                                var e = window.open(o.toDataURL(t.type), "_blank");
                                e.focus()
                            }
                            ;
                            var I = document.getElementById("img_" + a.id);
                            I.src = r.toDataURL(t.type)
                        } catch (k) {
                            x.error(S, x.errors.THUMB_PROCESS, k)
                        }
                    g(),
                    x.auto_upload && v(null),
                    delete o,
                    delete s
                }
                ,
                h.src = this.result
            } else if (x.image.thumb && t.type.match("gif")) {
                var h = document.getElementById("img_" + a.id);
                h.width = x.image.thumb_width,
                h.height = x.image.thumb_height,
                h.src = this.result,
                R.push({
                    data: this.result,
                    name: t.name,
                    id: a.id,
                    chunk: !1,
                    state: "ready"
                }),
                g()
            } else {
                if (x.icon.icon) {
                    var l = getFileExtension(t.name);
                    l = (endsWith(x.icon.path, "/") ? "" : "/") + l;
                    var h = document.getElementById("img_" + e);
                    h.width = x.icon.width,
                    h.height = x.icon.height,
                    h.onerror = function() {
                        h.src = location.protocol + "//" + location.host + x.icon._default,
                        h.onerror = void 0
                    }
                    ,
                    h.src = location.protocol + "//" + location.host + x.icon.path + l + x.icon.extension
                }
                R.push({
                    data: this.result,
                    name: t.name,
                    id: a.id,
                    chunk: !1,
                    state: "ready"
                }),
                g()
            }
        }
        ,
        i.readAsDataURL(t)
    }
    function f(e, t, i) {
        if (i.type.match("gif")) {
            var a = (document.getElementById(e),
            document.getElementById("img_" + e));
            a.width = x.image.thumb_width,
            a.height = x.image.thumb_height,
            a.src = t,
            R.push({
                data: t,
                name: i.name,
                id: e,
                chunk: !1,
                state: "ready"
            }),
            g()
        } else {
            var r = document.createElement("canvas")
              , n = r.getContext("2d")
              , o = document.createElement("canvas")
              , s = o.getContext("2d")
              , a = new Image;
            a.onload = function() {
                var t, h, l = 1;
                if (t = x.image.resize_width && 0 != x.image.resize_width ? x.image.resize_width : a.width,
                h = !x.image.keep_aspect_ratio && x.image.resize_height && 0 != x.image.resize_height ? x.image.resize_height : x.image.keep_aspect_ratio && x.image.resize_width && 0 != x.image.resize_width ? x.image.resize_width / (a.width / a.height) : a.height,
                a.width > x.image.thumb_width ? l = x.image.thumb_width / a.width : a.height > x.image.thumb_height && (l = x.image.thumb_height / a.height),
                o.width = t,
                o.height = h,
                s.drawImage(a, 0, 0, o.width, o.height),
                x.watermark.watermark) {
                    var m = document.createElement("canvas")
                      , c = m.getContext("2d");
                    if (m.width = t,
                    m.height = h,
                    x.watermark.image) {
                        var d = N.width
                          , u = N.height;
                        x.watermark.image_aspect_ratio && (d = N.width,
                        u = N.height / (t / h));
                        var p = 0
                          , f = u;
                        x.watermark.position.match("bottom") ? f = m.height - u : x.watermark.position.match("top") ? f = 0 : x.watermark.position.match("center") && (f = (m.height - u) / 2),
                        x.watermark.position.match("left") ? p = 0 : x.watermark.position.match("mid") ? p = (m.width - d) / 2 : x.watermark.position.match("right") && (p = m.width - d),
                        c.globalAlpha = x.watermark.alpha,
                        c.drawImage(N, p, f, d, u)
                    } else if (x.watermark.text) {
                        c.fillStyle = x.watermark.color,
                        c.font = x.watermark.font_size + " " + x.watermark.font,
                        c.globalAlpha = x.watermark.alpha;
                        var w = c.measureText(x.watermark.text)
                          , _ = x.watermark.font_size.match("px") ? x.watermark.font_size.replace("px", "") : x.watermark.font_size
                          , p = 0
                          , f = _;
                        x.watermark.position.match("bottom") ? f = m.height - .3 * _ : x.watermark.position.match("top") ? p = 0 + .3 * _ : x.watermark.position.match("center") && (f = (m.height - _) / 2),
                        x.watermark.position.match("left") ? p = 0 : x.watermark.position.match("mid") ? p = (m.width - w.width) / 2 : x.watermark.position.match("right") && (p = m.width - w.width),
                        c.translate(p, f),
                        c.fillText(x.watermark.text, 0, 0)
                    }
                    s.drawImage(m, 0, 0, m.width, m.height, 0, 0, m.width, m.height)
                }
                if (r.width = a.width * l,
                r.height = a.height * l,
                n.drawImage(o, 0, 0, o.width, o.height, 0, 0, r.width, r.height),
                x.image.thumb) {
                    var v = document.getElementById("img_" + e);
                    v.width = r.width,
                    v.height = r.height,
                    v.src = r.toDataURL(i.type);
                    try {
                        v.style.cursor = "pointer",
                        v.onclick = function() {
                            var e = window.open(o.toDataURL(i.type), "_blank");
                            e.focus()
                        }
                    } catch (I) {
                        x.error(S, x.errors.THUMB_PROCESS, I)
                    }
                }
                var k = o.toDataURL(i.type)
                  , E = k.split(",")
                  , b = b64toBytes([E[1]], i.type)
                  , T = new File(b,i.name);
                R.push({
                    data: T,
                    type: "image",
                    id: e,
                    chunk: !0,
                    lastPosition: 0,
                    state: "ready"
                }),
                g(),
                delete o,
                delete s
            }
            ,
            a.src = t
        }
    }
    function w(e, t) {
        if (x.image.thumb && t.type.match("image")) {
            var i = new FileReader;
            i.onload = function() {
                f(e, this.result, t)
            }
            ,
            i.readAsDataURL(t)
        } else if (R.push({
            data: t,
            type: "normal",
            id: e,
            chunk: !0,
            lastPosition: 0,
            state: "ready"
        }),
        x.icon.icon) {
            var a = getFileExtension(t.name);
            a = (endsWith(x.icon.path, "/") ? "" : "/") + a;
            var r = document.getElementById("img_" + e);
            r.width = x.icon.width,
            r.height = x.icon.height,
            r.onerror = function() {
                r.src = location.protocol + "//" + location.host + x.icon._default,
                r.onerror = void 0
            }
            ,
            r.src = location.protocol + "//" + location.host + x.icon.path + a + x.icon.extension
        }
        g()
    }
    function _(e) {
        if ("object" == typeof e)
            _(e.currentTarget.parentNode.id);
        else
            for (var t in R)
                if (R[t].id == e) {
                    R.splice(t, 1);
                    var i = document.getElementById(e);
                    x.imageHolder.removeChild(i)
                }
    }
    function v(e) {
        e && e.preventDefault();
        for (var t in R)
            "ready" == R[t].state && (R[t].state = "uploading",
            I(R[t]));
        return !1
    }
    function I(e) {
        console.log(e),
        e.chunk ? E(e) : k(e)
    }
    function k(e) {
        var t = new XMLHttpRequest;
        t.open("POST", x.ajax.url, !0),
        t.setRequestHeader("UploaderName", x.name),
        t.setRequestHeader("FileName", e.name),
        t.setRequestHeader("FileSize", e.data.length),
        t.setRequestHeader("Chunk", !1),
        t.onerror = b,
        t.upload.template_id = e.id,
        x.ajax.beforeSend && x.ajax.beforeSend(t),
        t.setRequestHeader("Content-type", e.data.split(",")[0]),
        t.onreadystatechange = y,
        t.upload.addEventListener("load", y, !1),
        t.upload.addEventListener("progress", T, !1),
        e.xhr = t,
        t.send(e.data)
    }
    function E(e) {
        var t = new XMLHttpRequest;
        t.open("POST", x.ajax.url, !0),
        t.onerror = b,
        t.upload.template_id = e.id,
        x.ajax.beforeSend && x.ajax.beforeSend(t),
        t.setRequestHeader("UploaderName", x.name),
        t.setRequestHeader("FileTemplateId", e.id),
        t.setRequestHeader("FileName", e.data.name),
        t.setRequestHeader("FileSize", e.data.size),
        t.setRequestHeader("Chunk", !0),
        t.setRequestHeader("ChunkSize", x.chunk.size),
        t.setRequestHeader("ChunkPosition", e.lastPosition);
        var i = e.data
          , a = e.lastPosition
          , r = i.slice(a, a + x.chunk.size);
        e.lastPosition = a + x.chunk.size,
        t.onreadystatechange = y,
        e.xhr = t,
        t.send(r)
    }
    function b(e) {
        x.error(S, x.errors.NETWORK, e)
    }
    function T(e) {
        if (x.progress) {
            var t = 100 * e.totalSize / e.position;
            x.progress({
                percent: t,
                template: e.target.template_id
            })
        }
    }
    function y(e) {
        var t = e.target.template_id;
        t || (t = e.target.upload.template_id);
        var i = S.find(t);
        if (null != i)
            if (i.chunk) {
                if (4 == e.target.readyState && 200 == e.target.status) {
                    var a = i.data.size
                      , r = i.lastPosition;
                    i.lastPosition > a ? (T({
                        totalSize: a,
                        position: a,
                        target: {
                            template_id: t
                        }
                    }),
                    x.ajax.clearAfterUpload && setTimeout(_, 1500, t),
                    x.success && x.success(e)) : (T({
                        totalSize: r,
                        position: a,
                        target: {
                            template_id: t
                        }
                    }),
                    "uploading" == i.state && e.totalSize == e.total && E(i))
                }
            } else
                4 == e.target.readyState && (200 == e.target.status ? (x.ajax.clearAfterUpload && setTimeout(_, 1500, t),
                x.success && (x.progress({
                    percent: 100,
                    template: t
                }),
                x.success(e))) : x.error(S, x.errors.NETWORK, e));
        else {
            var n = x.errors.FIND_ITEM.replace(/{id}/g, t);
            x.error(S, n)
        }
    }
    e = extend(!1, defaultSettings, e);
    var L, x = e, z = document.createElement("div"), F = [], R = [], S = this;
    if (this.settings = x,
    !isHTML5())
        return void e.html5Error(this);
    var N = new Image;
    return z.style.clear = "both",
    i(x) ? a(x) ? o(x) ? r(x) ? n(x) ? s(x) ? h(x) ? l(x) ? (t(x),
    x.dropPlace.onclick = function() {
        x.file_input.click()
    }
    ,
    x.form.addEventListener("submit", v, !1),
    this.find = function(e) {
        var t = null;
        for (var i in R)
            if (R[i].id == e) {
                t = R[i];
                break
            }
        return t
    }
    ,
    this.start = function(e, t) {
        var i = this.find(e);
        !i || "ready" != i.state && "paused" != i.state || (i.state = "uploading",
        I(i)),
        isFunction(t) && t()
    }
    ,
    this.pause = function(e, t) {
        var i = this.find(e);
        i && "uploading" == i.state && (i.state = "paused"),
        isFunction(t) && t()
    }
    ,
    this.remove = function(e, t) {
        var i = this.find(e);
        i && (i.xhr && i.xhr.abort && i.xhr.abort(),
        i.chunk && "uploading" == i.state ? i.state = "removed" : _(e)),
        isFunction(t) && t()
    }
    ,
    void (this.isworking = function() {
        var e = !1;
        for (var t in R)
            if ("uploading" == R[t].state || "paused" == R[t].state) {
                e = !0;
                break
            }
        return e
    }
    )) : printLog(x.errors.INIT_TEMPLATE) && !1 : printLog(x.errors.INIT_FILE_FORM) && !1 : printLog(x.errors.INIT_FILE_SIZE) && !1 : printLog(x.errors.INIT_FILE_FILTER) && !1 : printLog(x.errors.INIT_FILE_INPUT) && !1 : printLog(x.errors.INIT_IMG_THUMB) && !1 : printLog(x.errors.INIT_IMG_HOLDER) && !1 : printLog(x.errors.INIT_DROP) && !1
}
var defaultSettings = {
    name: "uploader",
    drop: !0,
    dropPlace: document.getElementById("dropPlace"),
    dragHoverClass: "hover",
    imageHolder: document.getElementById("imageHolder"),
    file_input: document.getElementById("fileInput"),
    file_filter: "image|text",
    file_size: 1048576e3,
    file_class: "file",
    template: '<div class="form-group text-center"><a href="javascript:void(0)" class="btn btn-danger close" onclick="{uploader}.remove(\'{id}\')">x</a>{image}</div><div class="form-group text-center"><div class="form-group col-md-6"><a href="javascript:void(0)" class="btn btn-info" onclick="{uploader}.start(\'{id}\')">Start</a></div><div class="form-group col-md-6"><a href="javascript:void(0)" class="btn btn-info" onclick="{uploader}.pause(\'{id}\')">Pause</a></div><p>{file.name} - {file.size}</p></div>',
    limit: 0,
    ajax: {
        url: "",
        clearAfterUpload: !0,
        beforeSend: function() {}
    },
    form: document.getElementById("imageForm"),
    image: {
        thumb: !0,
        thumb_width: 200,
        thumb_height: 200,
        resize_width: 0,
        resize_height: 0,
        keep_aspect_ratio: !1,
        preparing: "preparing.png"
    },
    watermark: {
        watermark: !0,
        image: "",
        image_aspect_ratio: !1,
        text: "www.yourweb.com",
        color: "grey",
        alpha: 1,
        font_size: "55px",
        font: "bold sans-serif",
        position: "mid center"
    },
    icon: {
        icon: !0,
        path: "/images/icons",
        _default: "/images/icons/_blank.png",
        width: 128,
        height: 128,
        extension: ".png"
    },
    auto_upload: !1,
    debug: !1,
    chunk: {
        active: !1,
        size: 1048576
    },
    errors: {
        INIT_DROP: "Drop Place can not initialize",
        INIT_IMG_HOLDER: "Image Holder can not initialize",
        INIT_IMG_THUMB: "Image Properties (thumb, width, height) can not initialize",
        INIT_FILE_INPUT: "File Input can not initialize",
        INIT_FILE_FILTER: "File Filter can not initialize",
        INIT_FILE_SIZE: "File Size can not initialize",
        INIT_FILE_FORM: "File Form can not initialize",
        INIT_TEMPLATE: "Your template content has not exist {image} tag",
        INIT_WATERMARK_IMAGE: "Watermark image can not initialize, watermark text will be on",
        THUMB_SIZE: "Thumbnail must be higher than 10x10",
        THUMB_PROCESS: "Something weird happened in thumbnail process",
        FILE_SIZE_INT: "File size must be integer",
        UPLOAD_LIMIT: "Upload limit is {limit}",
        FILE_SIZE_LARGER: "File size too larger",
        FILE_DOESNT_SUPPORT: "File does not support",
        TEMPLATE_CREATE: "Something weird happened in create of template, please check for details.",
        NETWORK: "Network error, please check for details.",
        FIND_ITEM: "There is no item for this id : {id}"
    },
    html5Error: function() {
        printLog("No HTML5 support")
    },
    success: function() {
        printLog("Pictures sended.")
    },
    progress: function(e) {
        printLog("Picture " + e)
    },
    error: function() {}
}
  , isLocalStorage = function() {
    try {
        return localStorage.setItem(mod, mod),
        localStorage.removeItem(mod),
        !0
    } catch (e) {
        return !1
    }
}
  , isCanvas = function() {
    var e = document.createElement("canvas");
    return !(!e.getContext || !e.getContext("2d"))
}
  , isCanvasText = function() {
    return !(!isCanvas() || !is(document.createElement("canvas").getContext("2d").fillText, "function"))
}
  , isHTML5 = function() {
    return isCanvasText && isLocalStorage && window.File && window.FileReader && window.FileList && window.Blob && window.FormData
};
