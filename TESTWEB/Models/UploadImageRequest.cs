using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TESTWEB.Models
{
    public enum VerticalClipAlign
    {
        Start, End, Center
    }

    public enum HorizontalClipAlign
    {
        Start, End, Center
    }

    public class UploadImageRequest
    {
        [JsonProperty(PropertyName = "width_percent", Required = Required.Always)]
        public int WidthPercent { get; set; }
        [JsonProperty(PropertyName = "height_percent", Required = Required.Always)]
        public int HeightPercent { get; set; }
        [JsonProperty(PropertyName = "base64_image", Required = Required.Always)]
        public string Base64Image { get; set; }
        [JsonProperty(PropertyName = "vertical_clip_align", Required = Required.Always)]
        public VerticalClipAlign VerticalClipAlign { get; set; }
        [JsonProperty(PropertyName = "horizontal_clip_align", Required = Required.Always)]
        public HorizontalClipAlign HorizontalClipAlign { get; set; }
    }
}