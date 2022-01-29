using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MusicHUB.Models
{
    [Serializable]
    [XmlRoot("artist")]
    public class Artist
    {
        [XmlElement("name")]
       // [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [XmlElement("mbid")]
        //[JsonProperty(PropertyName = "mbid")]
        public string Mbid { get; set; }
        [XmlElement("url")]
        //[JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [XmlElement("image")]
        //[JsonProperty(PropertyName = "image")]
        public Image Image { get; set; }
        [XmlElement("streamable")]
       // [JsonProperty(PropertyName = "streamable")]
        public bool Streamable { get; set; }
        [XmlElement("ontour")]
        //[JsonProperty(PropertyName = "ontour")]
        public bool OnTour { get; set; }
        [XmlElement("stats")]
        //[JsonProperty(PropertyName = "stats")]
        public Stats Stats { get; set; }
        [XmlElement("bio")]
        //[JsonProperty(PropertyName = "bio")]
        public Bio Bio { get; set; }
        [XmlElement("tags")]
        //[JsonProperty(PropertyName = "tags")]
        public Tag[] Tags { get; set; }
        [XmlElement("similar")]
        //[JsonProperty(PropertyName = "similar")]
        public List<Artist> Similar { get; set; }
    }

    public class Image
    {
        [XmlElement("#text")]
        //[JsonProperty(PropertyName = "#text")]
        public string uri { get; set; }
        [XmlElement("size")]
        //[JsonProperty(PropertyName = "size")]
        public Size size { get; set; }
    }

    public class Stats
    {
        [XmlElement("listeners")]
        //[JsonProperty(PropertyName = "listeners")]
        public int Listeners { get; set; }
        [XmlElement("playcount")]
        //[JsonProperty(PropertyName = "playcount")]
        public int PlayCount { get; set; }
    }

    public class Tag
    {
        [XmlElement("name")]
        //[JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [XmlElement("url")]
        //[JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }

    public class Bio
    {
        [XmlElement("links")]
        public Link Links { get; set; }

        [XmlElement("published")]
        //[JsonProperty(PropertyName = "published")]
        public DateTimeOffset Published { get; set; }

        [XmlElement("summary")]
        //[JsonProperty(PropertyName = "summary")]
        public string Summary { get; set; }

        [XmlElement("content")]
        //[JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
    }

    public class Link
    {
        [XmlElement("#text")]
        //[JsonProperty(PropertyName = "#text")]
        public string Text { get; set; }

        [XmlElement("rel")]
        //[JsonProperty(PropertyName = "rel")]
        public string Rel { get; set; }

        [XmlElement("href")]
        //[JsonProperty(PropertyName = "href")]
        public string uri { get; set; }
    }

    public enum Size
    {
        Small,
        Medium,
        Large,
        ExtraLarge,
        Mega,
        Largest
    }
}
