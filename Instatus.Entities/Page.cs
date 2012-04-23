using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Spatial;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Instatus.Entities
{   
    public class Page
    {
        public int Id { get; set; }
        public int Locale { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime PublishedTime { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public Source Source { get; set; }
        public Card Card { get; set; }
        public Location Location { get; set; }
        public Schedule Schedule { get; set; }
        public Availability Availability { get; set; }
        public User User { get; set; }
        public Application Application { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Association> Associations { get; set; }
#if NET45
        public Kind Kind { get; set; }
        public Published Published { get; set; }
        public Privacy Privacy { get; set; }
#else
        public string Kind { get; set; }
        public string Published { get; set; }
        public string Privacy { get; set; }
#endif

        public Page()
        {
            Source = new Source();
            Card = new Card();
            Location = new Location();
            Schedule = new Schedule();
            Availability = new Availability();
            CreatedTime = DateTime.UtcNow;
            UpdatedTime = CreatedTime;
            PublishedTime = CreatedTime;
        }
    }

    public enum Provider
    {
        Facebook,
        Twitter,
        Google,
        GoogleAnalytics,
        Generated,
        Custom1,
        Custom2,
        Custom3
    }

    public enum Kind
    {
        Achievement,
        Album,
        Article,
        Brand,
        Campaign,
        CaseStudy,
        Catalog,
        Event,
        Group,
        Post,
        Place,
        Product,
        Profile,
        Offer,
        Organization,
        Job,
        News,
        Note,
        Listing,
        Milestone,
        Region
    }

    public enum Published
    {
        Active,
        Draft,
        Spam
    }

    public enum Privacy
    {
        Public,
        Private
    }

    [ComplexType]
    public class Source
    {
        public string Uri { get; set; }
#if NET45
        public Provider Provider { get; set; }
#else
        public string Provider { get; set; }
#endif
    }

    [ComplexType]
    public class Card
    {
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string EmailAddress { get; set; }
    }

    [ComplexType]
    public class Location
    {
        public string Name { get; set; }
        public string FormattedAddress { get; set; }
        public string StreetAddress { get; set; }
        public string Locality { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public double ZoomLevel { get; set; }
#if NET45
        public DbGeography Spatial { get; set; }
#else
        public double Latitude { get; set; }
        public double Longitude { get; set; }
#endif
    }

    [ComplexType]
    public class Schedule
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

    [ComplexType]
    public class Availability
    {
        public float? Price { get; set; }
    }
}
