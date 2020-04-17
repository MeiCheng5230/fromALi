import MegaPixImage from 'MegaPixImage'
import JPEGEncoder from 'JPEGEncoder'

// 将base64转换为blob
export function convertBase64UrlToBlob(urlData) {
  var arr = urlData.split(',')
  var mime = arr[0].match(/:(.*?);/)[1]
  var bstr = atob(arr[1])
  var n = bstr.length
  var u8arr = new Uint8Array(n)
  while (n--) {
    u8arr[n] = bstr.charCodeAt(n)
  }
  return new Blob([u8arr], { type: mime })
}

// 压缩图片
export function compressImage(path, config) {
  return new Promise((resolve, reject) => {
    // 生成canvas
    var canvas = document.createElement('canvas')
    var ctx = canvas.getContext('2d')
    var img = new Image()
    img.crossOrigin = 'Anonymous'
    img.onload = function() {
      var that = this
      var w = that.width
      var h = that.height
      var scale = w / h
      w = config.width || config.height * scale || w
      h = config.height || config.width / scale || h
      // 最大宽高如有限制时的处理
      w = config.maxWidth && w > config.maxWidth ? config.maxWidth : w
      h = config.maxHeight && h > config.maxHeight ? config.maxHeight : h
      w = Math.min(w, h * scale) || w
      h = Math.min(h, w / scale) || h

      var quality = 0.7 // 默认图片质量为0.7
      // 创建属性节点
      var anw = document.createAttribute('width')
      anw.nodeValue = w
      var anh = document.createAttribute('height')
      anh.nodeValue = h
      canvas.setAttributeNode(anw)
      canvas.setAttributeNode(anh)
      ctx.drawImage(that, 0, 0, w, h)
      if (config.quality && config.quality <= 1 && config.quality > 0) {
        quality = config.quality
      }
      var base64 = canvas.toDataURL('image/jpeg', quality)
      // 在指定图片格式为 image/jpeg 或 image/webp的情况下，可以从 0 到 1 的区间内选择图片的质量。如果超出取值范围，将会使用默认值 0.92。其他参数会被忽略。

      // 修复IOS
      if (navigator.userAgent.match(/iphone/i)) {
        var mpImg = new MegaPixImage(img)
        mpImg.render(canvas, {
          maxWidth: w,
          maxHeight: h,
          quality: quality
        })
        base64 = canvas.toDataURL('image/jpeg', quality)
      }

      // 修复android
      if (navigator.userAgent.match(/Android/i)) {
        var encoder = new JPEGEncoder()
        base64 = encoder.encode(ctx.getImageData(0, 0, w, h), quality * 100)
      }

      // var blob = convertBase64UrlToBlob(base64)
      // 回调函数返回base64的值，也可根据自己的需求返回blob的值

      // base64 = base64.substr(base64.indexOf(',') + 1)
      // 去掉base64前面的部分

      resolve(base64)
      canvas = null
    }
    img.src = path
  })
}