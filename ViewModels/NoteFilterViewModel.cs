using System;
using System.ComponentModel.DataAnnotations;

namespace NotesApplication.ViewModels
{
    public class NoteFilterViewModel
    {
        public string TitleSearch { get; set; }
        public string ContentSearch { get; set; }
        
        public SortOrder CreatedAtSortOrder { get; set; } = SortOrder.Descending;
        public SortOrder UpdatedAtSortOrder { get; set; } = SortOrder.None;
        
        public bool HasFilters => !string.IsNullOrEmpty(TitleSearch) || !string.IsNullOrEmpty(ContentSearch);
    }

    public enum SortOrder
    {
        None,
        Ascending,
        Descending
    }
}