using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

var book = new Book
{
    Title = "The Great Gatsby",
    Author = "F. Scott Fitzgerald",
    Year = 1925
};


var serializer = new XmlSerializer(typeof(Book));
using (var writer = new StringWriter())
{
    serializer.Serialize(writer, book);
    string xmlOutput = writer.ToString();
    Console.WriteLine(xmlOutput);
}



[XmlRoot("Library")]
class Book
{
    [XmlElement("Title")]
    public string Title { get; set; }
    [XmlElement("Author")]
    public string Author { get; set; }
    [XmlElement("Year")]
    public int Year { get; set; }
}