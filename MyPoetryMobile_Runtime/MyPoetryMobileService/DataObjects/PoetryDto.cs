using Microsoft.Azure.Mobile.Server;
using System;

namespace MyPoetryMobileService.DataObjects
{
    public class PoetryDto : EntityData
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        public int CharactersNumber { get; set; }
        public int WordsNumber { get; set; }
        public int VersesNumber { get; set; }
        public int Rating { get; set; }
        public bool BookmarkYN { get; set; }
    }
}