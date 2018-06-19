# MediaService

REST API 设计

1.  `POST /image/Upload`
  + 输入：Body 包含图片文件内容，可以为 `.jpg`, `.png`, `.gif` 等常见图片格式         
  + 返回：报文体包含下载文件的绝对地址，格式是 `/image/Download/{id}`, 如 `http://localhost:52110/Image/Download/5b239c329bc13845f061487d`
  + 如果不是图片，返回 `invalid file format`        
2.  `GET /image/Download/{id}`
  + 输入：`{id}` 是 `POST /image/Upload` 返回的 id 部分。        
  + 返回：图片文件。如果 id 不存在，返回 `404 Not Found`。
3.  `GET /image/Download/{id}?xmin={xmin}&xmax={xmax}&ymin={ymin}&ymax={ymax}`
  + 输入：`{id}` 是 `POST /image/Upload` 返回的 id 部分，(`{xmin}`, `{xmax}`, `{ymin}`, `{ymax}`) 为需要截取的矩形区域范围。
  + 返回：经过裁剪的图片文件，此法不会改变服务器存储图片的内容。如果 xmin >= xmax 或者 ymin >= ymax 以及其它不合法的情况，返回 `invalid arguments` 
4.  `GET /image/Info/{id}`
  + 输入：`{id}` 是 `POST /image/Upload` 返回的 id 部分。
  + 返回：图片信息，格式为：
  
  ```
  {
    "id": "5b2399145384d72ba008bfda",
    "width": 2902,
    "height": 1888,
    "MD5": "a0dca0e62fb6d1d2",
    "blobName": "A0DCA0E62FB6D1D2.png"
  }
  ```
  `id` 为图片 id， `width` 为图片宽度，`height` 为图片的高度，`MD5` 为图片文件的 MD5 的低 64 位 16 进制值，`blobName` 是 Azure Blob 的 blob 名称。        
  如果 `id` 不存在，返回 `404 Not Found`。
  
