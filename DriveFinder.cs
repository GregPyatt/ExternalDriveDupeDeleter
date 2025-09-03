using System;
using System.IO;
using System.Linq;

/// <summary>
/// A helper class to find and get information about external drives.
/// </summary>
/// 
namespace ExternalDriveDupeDeleter { 
    public static class DriveFinder
    {
        /// <summary>
        /// Finds the drive letter of an external drive by its volume label (name).
        /// </summary>
        /// <param name="driveName">The name (volume label) of the external drive to find.</param>
        /// <returns>The drive letter (e.g., "E:\") if found; otherwise, returns an empty string.</returns>
        public static string GetDriveLetterByName(string driveName)
        {
            try
            {
                // Get the path of the system drive (e.g., "C:\")
                string systemDrivePath = Path.GetPathRoot(Environment.GetEnvironmentVariable("windir")) ?? "C:\\";

                // Get all drives connected to the system.
                DriveInfo[] allDrives = DriveInfo.GetDrives();

                var externalDrive = allDrives.FirstOrDefault(drive =>
                            drive.IsReady &&
                            // We no longer check for DriveType.Removable
                            !drive.Name.Equals(systemDrivePath, StringComparison.OrdinalIgnoreCase) && // Exclude the OS drive
                            drive.VolumeLabel.Equals(driveName, StringComparison.OrdinalIgnoreCase)); // Match the name


                // If a drive was found, return its     root directory path (e.g., "E:\").
                // Otherwise, return an empty string to indicate it wasn't found.
                return externalDrive?.Name ?? string.Empty;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while accessing drive information: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the volume label (name) of a drive from its drive letter.
        /// </summary>
        /// <param name="driveLetter">The drive letter to look up (e.g., "E" or "E:\").</param>
        /// <returns>The volume label of the drive if found; otherwise, returns an empty string.</returns>
        public static string GetDriveNameByLetter(string driveLetter)
        {
            try
            {
                // Ensure the drive letter is in the correct format (e.g., "E:\").
                if (!driveLetter.EndsWith(":\\"))
                {
                    driveLetter = $"{driveLetter.ToUpper()}:\\";
                }

                // Get all drives and find the one that matches the letter.
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                var targetDrive = allDrives.FirstOrDefault(drive =>
                    drive.IsReady &&
                    drive.Name.Equals(driveLetter, StringComparison.OrdinalIgnoreCase));

                // Return the drive's volume label or an empty string if not found.
                return targetDrive?.VolumeLabel ?? string.Empty;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while accessing drive information: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
