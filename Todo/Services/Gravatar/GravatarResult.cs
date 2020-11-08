using System;

namespace Todo.Services.Gravatar
{
    public class GravatarResult
    {
        public Entry[] Entry { get; set; }

        public string GetDisplayName()
        {
            if (Entry.Length == 0)
                throw new GravatarResultHasNoEntriesError();

            return Entry[0].DisplayName;
        }
    }

    public class Entry
    {
        public string DisplayName { get; set; }
    }

    public class GravatarResultHasNoEntriesError : Exception
    {
    }
}
