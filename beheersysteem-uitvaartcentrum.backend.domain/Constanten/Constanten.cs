using System;
using System.Collections.Generic;
using System.Text;

namespace beheersysteem_uitvaartcentrum.backend.domain.Constanten
{
    public static class Constanten
    {
        public static int MaxFileSizeInBytes => 500 * 1024 * 1024; // 500MB
        public static List<string> AllowedFileExtensions => new List<string> { ".pdf", ".jpg", ".jpeg", ".png" };
    }
}
