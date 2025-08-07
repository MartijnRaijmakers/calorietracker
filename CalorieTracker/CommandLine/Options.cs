using CommandLine;

namespace App.CommandLine
{
    internal class Options
    {
        // Required arguments
        [Option('a', "action", Required = true, HelpText = $"The action to execute, either 'create' or 'get'")]
        public string Action { get; set; }

        // Optional arguments needed for one or more functional paths
        [Option("name", Required = false, HelpText = "Name of the account holder")]
        public string? Name { get; set; }

        [Option("lastName", Required = false, HelpText = "Last name of the account holder (always optional)")]
        public string? LastName { get; set; }

        [Option("dateOfBirth", Required = false, HelpText = "Date of birth of the account holder (format: yyyy-MM-dd)")]
        public DateOnly? DateOfBirth { get; set; }

        [Option("gender", Required = false, HelpText = "Gender of the account holder")]
        public string? Gender { get; set; }

        [Option("height", Required = false, HelpText = "Height of the account holder in cm")]
        public decimal? Height { get; set; }

        [Option("weight", Required = false, HelpText = "Weight of the account holder in kg")]
        public decimal? Weight { get; set; }
    }
}
