using System;

using SidePhotoViewer.Models;

namespace SidePhotoViewer.Contracts.Services
{
    public interface IThemeSelectorService
    {
        bool SetTheme(AppTheme? theme = null);

        AppTheme GetCurrentTheme();
    }
}
