﻿namespace bvnote_web_api.Data.DTO
{
    public record AbbrevDTO
    {
        public string? AbbrevId { get; set; }
        public string? Abbreviation { get; set; }
        public string? BookId { get; set; }

        // TODO: apply asynchronous
        public static List<AbbrevDTO> GetAbbrevDTOs(List<Abbrev> abbrevs)
        {
            return abbrevs.Select(abbrev => new AbbrevDTO
            {
                AbbrevId = abbrev.AbbrevId,
                Abbreviation = abbrev.Abbreviation,
                BookId = abbrev.BookId
            }).ToList();
        }
    }
}
