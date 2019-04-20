namespace MyComparer.Test
{
    using MyComparer.Hashers.MD5s;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MyList;

    class Program
    {
        public static void M1()
        {
            var directoryInfo = new System.IO.DirectoryInfo("U:\\Cz\\a\\BackUp");
            global::MyList.DAL.Entities.Directory directory = directoryInfo;

            using (var myEqualityComparer = new MyComparer.Hashers.MD5s.MemoryParallel())
            {
                var duplicateFiles = directory.DuplicateFiles(myEqualityComparer);
            }
        }

        public static void M2()
        {
            var directoryInfo = new System.IO.DirectoryInfo("U:\\Cz\\a\\BackUp");
            global::MyList.DAL.Entities.Directory directory = directoryInfo;

            using (var myEqualityComparer = Hashers.MD5s.Linear.NewInCloud())
            {
                var duplicateFiles = directory.DuplicateFiles(myEqualityComparer);
            }
        }

        public static void M3()
        {
            var directoryInfo = new System.IO.DirectoryInfo("U:\\Cz\\a\\BackUp");
            global::MyList.DAL.Entities.Directory directory = directoryInfo;

            using (var myEqualityComparer = Hashers.MD5s.Linear.NewInMemory())
            {
                var duplicateFiles = directory.DuplicateFiles(myEqualityComparer);
            }
        }

        public static void M4()
        {
            var directoryInfo = new System.IO.DirectoryInfo("U:\\#newC");
            global::MyList.DAL.Entities.Directory directory = directoryInfo;
            var guid = Guid.NewGuid();
            System.Diagnostics.Debug.WriteLine("guid: " + guid);

            using (var myEqualityComparer = new Hashers.MD5s.Cloud(guid))
            {
                var duplicateFiles = directory.DuplicateFiles(myEqualityComparer);
            }
        }

        public static void M5()
        {
            var directoryInfo = new System.IO.DirectoryInfo("U:\\Cz\\a\\BackUp");
            global::MyList.DAL.Entities.Directory directory = directoryInfo;
            //var guid = Guid.NewGuid();
            var guid = new Guid("c15a50ba-e925-45c2-9468-e17c68ff8ff0");

            using (var myEqualityComparer = new Hashers.MD5s.Cloud(guid))
            {
                //var duplicateFiles = directory.DuplicateFiles(myEqualityComparer);

                System.Diagnostics.Debug.WriteLine("delete db records");

                myEqualityComparer.ReloadDirectories();
            }

            System.Diagnostics.Debug.WriteLine("delete db records");

            using (var myEqualityComparer = new Hashers.MD5s.Cloud(guid))
            {
                //var duplicateFiles = directory.DuplicateFiles(myEqualityComparer);
                myEqualityComparer.ReloadFiles();
            }
        }

        public static bool Reload()
        {
            var guid = new Guid("A1FC727B-BC81-489F-B784-C4C917DC57B0");

            using (var sql = new Hashers.DAL.Clouds.FileHash(guid))
            {
                var memory = new List<Hashers.DAL.Entities.FileHash>(sql);
                if (memory.Count >= 218406)
                    return false;
            }

            try
            {
                using (var myEqualityComparer = new Hashers.MD5s.Cloud(guid))
                {
                    myEqualityComparer.ReloadFiles(f=> f.Name != "Thumbs" && f.Extension != "db");
                }

            }
            //System.InvalidOperationException: 'This SqlTransaction has completed; it is no longer usable.'
            catch (Exception ex)
            {
                if(1==2)
                    throw ex;
            }

            return true;
        }

        public static bool ContainsFilePathStartWith(string filePath, IEnumerable<string> filePaths)
        {
            foreach(var path in filePaths)
            {
                if (filePath.StartsWith(path))
                {
                    return true;
                }
            }

            return false;
        }

        public static void deleteThumb(System.IO.FileInfo file)
        {
            if(file.Name == "Thumbs.db")
            {
                try
                {
                    file.Attributes &= ~System.IO.FileAttributes.ReadOnly;
                    file.Delete();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("############################");
                    System.Diagnostics.Debug.WriteLine(file.FullName);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine("############################");
                }
            }
        }

        public static void deleteThumbs()
        {
            var dir = new System.IO.DirectoryInfo("U:\\");
            var directories = new System.Collections.Concurrent.ConcurrentBag<System.IO.DirectoryInfo>();
            var exceptions = new System.Collections.Concurrent.ConcurrentBag<Exception>();

            dir.ListAllFilesParallel((file) => deleteThumb(file), exceptions.Add);

        }

        public static void AllFolder2()
        {
            var guid = new Guid("0baf23ef-fd7c-4e05-ba02-67f9e09889f8");

            try
            {
                using (var myEqualityComparer = new Hashers.MD5s.Cloud(guid))
                {
                    //var pathSftw = new string[]
                    //{
                    //    "\\A\\H\\DAVID\\",
                    //    "\\C\\W\\bkpz\\DAVID\\",
                    //    "\\Cz\\a\\DAVID\\"
                    //};

                    var filesHashes = myEqualityComparer
                        .FileHashes
                        .ToMemory()
                        //.Where(fh => ContainsFilePathStartWith(fh.File.Path, pathSftw))
                        //.Where(fh => System.IO.File.Exists(fh.File.GetFullPathAndName()))
                        .GroupBy(fh => fh.Hash)
                        .ToList();

                    foreach (var fileGrouped in filesHashes)
                    {
                        var deleteFiles = fileGrouped.Where(fg => fg.File.Path.StartsWith("\\#newC\\#zip\\"));

                        if (deleteFiles.Count() < fileGrouped.Count())
                        {
                            foreach (var file in deleteFiles)
                            {
                                var fullName = file.File.GetFullPathAndName();
                                try
                                {
                                    if (System.IO.File.Exists(fullName))
                                    {
                                        System.IO.File.Delete(fullName);
                                        System.Diagnostics.Debug.WriteLine("del...ing " + fullName);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine("############################");
                                    System.Diagnostics.Debug.WriteLine(fullName);
                                    System.Diagnostics.Debug.WriteLine(ex.Message);
                                    System.Diagnostics.Debug.WriteLine("############################");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (1 == 2)
                    throw ex;
            }
        }

        public static void AllFolder3()
        {
            var guid = new Guid("244853e2-61bf-4c5a-92f1-8ca82edf1702");

            try
            {
                using (var myEqualityComparer = new Hashers.MD5s.Cloud(guid))
                {
                    var filesHashes = myEqualityComparer
                        .FileHashes
                        .ToMemory()
                        .GroupBy(fh => fh.Hash)
                        .ToList();

                    foreach (var fileGrouped in filesHashes)
                    {
                        var first = fileGrouped.FirstOrDefault();

                        if (first == null)
                            continue;

                        var firstPath = first.File.Path;

                        foreach (var file in fileGrouped)
                        {
                            if (file.File.Path.Equals(firstPath) || !file.File.Path.StartsWith("\\#newC\\#zip\\"))
                                continue;

                            var fullName = file.File.GetFullPathAndName();
                            try
                            {
                                if (System.IO.File.Exists(fullName))
                                {
                                    System.IO.File.Delete(fullName);
                                    System.Diagnostics.Debug.WriteLine("del...ing " + fullName);
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("############################");
                                System.Diagnostics.Debug.WriteLine(fullName);
                                System.Diagnostics.Debug.WriteLine(ex.Message);
                                System.Diagnostics.Debug.WriteLine("############################");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (1 == 2)
                    throw ex;
            }
        }

        public static void AllOldFolder3()
        {
            var guid = new Guid("6ead3011-9d8c-4b92-a23c-ee2655d9cfe4");

            try
            {
                using (var myEqualityComparer = new Hashers.MD5s.Cloud(guid))
                {
                    var filesHashes = myEqualityComparer
                        .FileHashes
                        .ToMemory()
                        .Where(fh => fh.File.Path.IndexOf("\\#old") > 0)
                        .GroupBy(fh => fh.Hash + fh.File.Path.Substring(0, fh.File.Path.IndexOf("\\#old") + 5))
                        .ToList();

                    foreach (var fileGrouped in filesHashes)
                    {
                        var firstFile = fileGrouped.FirstOrDefault()?.File;
                        if (firstFile == null)
                            continue;

                        foreach (var fileHash in fileGrouped)
                        {
                            if (fileHash.File == firstFile)
                                continue;

                            var fullName = fileHash.File.GetFullPathAndName();
                            try
                            {
                                if (System.IO.File.Exists(fullName))
                                {
                                    System.IO.File.SetAttributes(fullName, ~System.IO.FileAttributes.ReadOnly);
                                    System.IO.File.Delete(fullName);
                                    System.Diagnostics.Debug.WriteLine("del...ing " + fullName);
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("############################");
                                System.Diagnostics.Debug.WriteLine(fullName);
                                System.Diagnostics.Debug.WriteLine(ex.Message);
                                System.Diagnostics.Debug.WriteLine("############################");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (1 == 2)
                    throw ex;
            }
        }

        public static void AllOldFolder4()
        {
            var guid = new Guid("c25086a2-765f-4ea3-abd5-257b63c665bc");

            try
            {
                using (var myEqualityComparer = new Hashers.MD5s.Cloud(guid))
                {
                    var filesHashes = myEqualityComparer
                        .FileHashes
                        .ToMemory()
                        //.Where(fh => fh.File.Path.IndexOf("\\#old") > 0)
                        .GroupBy(fh => fh.Hash + fh.File.Name)
                        .Where(fhg=> fhg.Where(fh=> fh.File.Path.Contains("\\#old")).Any())
                        .ToList();

                    foreach (var fileGrouped in filesHashes)
                    {
                        foreach (var fileHash in fileGrouped)
                        {
                            var oldIndex = fileHash.File.Path.IndexOf("\\#old");

                            if (oldIndex <= 0)
                                continue;

                            var asdf = fileGrouped.Where(f => f.File.Path.StartsWith(fileHash.File.Path.Substring(0, oldIndex))).ToList();
                            var asdf2 = asdf.Where(f => f.File.Path.Contains("\\#old")).Count();
                            if (asdf.Count() <= 1)
                                continue;

                            //if (asdf.Count() % asdf2 > 0) // something wrong because has already removed duplicated inside #old folder
                            //    return;
                            
                            var fullName = fileHash.File.GetFullPathAndName();
                            try
                            {
                                if (System.IO.File.Exists(fullName))
                                {
                                    System.IO.File.SetAttributes(fullName, ~System.IO.FileAttributes.ReadOnly);
                                    System.IO.File.Delete(fullName);
                                    System.Diagnostics.Debug.WriteLine("del...ing " + fullName);
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("############################");
                                System.Diagnostics.Debug.WriteLine(fullName);
                                System.Diagnostics.Debug.WriteLine(ex.Message);
                                System.Diagnostics.Debug.WriteLine("############################");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (1 == 2)
                    throw ex;
            }
        }

        public static void AllDownloadsRealPlayerFolder4()
        {
            var guid = new Guid("6ed7f788-845c-48ff-b424-e39cacb2a565");

            try
            {
                using (var myEqualityComparer = new Hashers.MD5s.Cloud(guid))
                {
                    var filesHashes = myEqualityComparer
                        .FileHashes
                        .ToMemory()
                        .Where(fh => fh.File.Path.Contains(@"#newC\OutrosAgrupados\New folder\Downloads do RealPlayer"))
                        .GroupBy(fh => fh.Hash)
                        .ToList();

                    foreach (var fileGrouped in filesHashes)
                    {
                        var hasFirst = false;
                        foreach (var fileHash in fileGrouped)
                        {
                            var fullName = fileHash.File.GetFullPathAndName();

                            if (!System.IO.File.Exists(fullName))
                                continue;

                            if (!hasFirst)
                            {
                                hasFirst = true;
                                continue;
                            }

                            try
                            {
                                System.IO.File.SetAttributes(fullName, ~System.IO.FileAttributes.ReadOnly);
                                System.IO.File.Delete(fullName);
                                System.Diagnostics.Debug.WriteLine("del...ing " + fullName);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("############################");
                                System.Diagnostics.Debug.WriteLine(fullName);
                                System.Diagnostics.Debug.WriteLine(ex.Message);
                                System.Diagnostics.Debug.WriteLine("############################");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (1 == 2)
                    throw ex;
            }
        }

        public static void AllCloudFolder4()
        {
            var guid = new Guid("A1FC727B-BC81-489F-B784-C4C917DC57B0");

            try
            {
                using (var myEqualityComparer = new Hashers.MD5s.Cloud(guid))
                {
                    var filesHashes = myEqualityComparer
                        .FileHashes
                        .ToMemory()
                        .GroupBy(fh => fh.Hash)
                        .Where(fg => fg.Count() > 1)
                        .ToList();

                    foreach (var fileGrouped in filesHashes)
                    {
                        var first = fileGrouped.Where(fh => !fh.File.Path.Contains(@"#newC\#CloudDrive")).FirstOrDefault();
                        if (first == null)
                            first = fileGrouped.FirstOrDefault();

                        foreach (var fileHash in fileGrouped)
                        {
                            var fullName = fileHash.File.GetFullPathAndName();

                            if (!System.IO.File.Exists(fullName))
                                continue;

                            if (fileHash.File == first.File || !fileHash.File.Path.Contains(@"#newC\#CloudDrive"))
                                continue;

                            try
                            {
                                System.IO.File.SetAttributes(fullName, ~System.IO.FileAttributes.ReadOnly);
                                System.IO.File.Delete(fullName);
                                System.Diagnostics.Debug.WriteLine("del...ing " + fullName);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("############################");
                                System.Diagnostics.Debug.WriteLine(fullName);
                                System.Diagnostics.Debug.WriteLine(ex.Message);
                                System.Diagnostics.Debug.WriteLine("############################");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (1 == 2)
                    throw ex;
            }
        }

        public static void AllArrumarFolder4()
        {
            var guid = new Guid("3c471149-265b-4770-bcd6-80b55c54f761");

            try
            {
                using (var myEqualityComparer = new Hashers.MD5s.Cloud(guid))
                {
                    var filesHashes = myEqualityComparer
                        .FileHashes
                        .ToMemory()
                        .GroupBy(fh => fh.Hash)
                        .Where(fg => fg.Count() > 1)
                        .ToList();

                    foreach (var fileGrouped in filesHashes)
                    {
                        var first = fileGrouped.Where(fh => !fh.File.Path.Contains(@"#newC\arrumar2\arrumar")).FirstOrDefault();
                        if (first == null)
                            first = fileGrouped.FirstOrDefault();

                        foreach (var fileHash in fileGrouped)
                        {
                            var fullName = fileHash.File.GetFullPathAndName();

                            if (!System.IO.File.Exists(fullName))
                                continue;

                            if (fileHash.File == first.File || !fileHash.File.Path.Contains(@"#newC\arrumar2\arrumar"))
                                continue;

                            try
                            {
                                System.IO.File.SetAttributes(fullName, ~System.IO.FileAttributes.ReadOnly);
                                System.IO.File.Delete(fullName);
                                System.Diagnostics.Debug.WriteLine("del...ing " + fullName);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("############################");
                                System.Diagnostics.Debug.WriteLine(fullName);
                                System.Diagnostics.Debug.WriteLine(ex.Message);
                                System.Diagnostics.Debug.WriteLine("############################");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (1 == 2)
                    throw ex;
            }
        }

        public static void AllEmptyFolders()
        {
            try
            {
                var sftwPaths = new string[] {
                    "U:\\#newC\\"
                };

                foreach (var basedir in sftwPaths)
                {
                    if (!System.IO.Directory.Exists(basedir))
                        continue;

                    var dir = new System.IO.DirectoryInfo(basedir);
                    var directories = new System.Collections.Concurrent.ConcurrentBag<System.IO.DirectoryInfo>();
                    var exceptions = new System.Collections.Concurrent.ConcurrentBag<Exception>();

                    dir.ListAllParallel(directories.Add, exceptions.Add);

                    var continueLoop = true;

                    while (continueLoop)
                    {
                        continueLoop = false;

                        foreach (var d in directories)
                        {
                            if (!System.IO.Directory.Exists(d.FullName))
                                continue;

                            if (!d.GetFiles().Any() && !d.GetDirectories().Any())
                            {
                                d.Attributes &= ~System.IO.FileAttributes.ReadOnly;
                                d.Delete();
                                System.Diagnostics.Debug.WriteLine("del...ing " + d.FullName);
                                continueLoop = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (1 == 2)
                    throw ex;
            }
        }

        public static void FixFileNames()
        {
            var guid = new Guid("605fa620-60de-466f-8f25-e92794478efd");

            using (var myEqualityComparer = new Cloud(guid))
            {
                var filesHashes = myEqualityComparer
                    .FileHashes
                    .ToMemory()
                    .GroupBy(fh => fh.Hash)
                    .Where(fhg => fhg.Count() > 1)
                    .ToList();

                var continueLoop = true;

                foreach (var filesHashGrouped in filesHashes)
                {
                    var ignoreNames = new List<string>();

                    foreach (var fileHash in filesHashGrouped)
                    {
                        if (continueLoop && fileHash.File.GetFullPathAndName() != @"U:\#newC\OutrosAgrupados\New folder\Downloads do RealPlayer\taking a ride - Free Porn Videos - YouPorn.com Lite (BETA)7.meta")
                            continue;

                        continueLoop = false;

                        var fileHashFullName = fileHash.File.GetFullName();

                        if (ignoreNames.Contains(fileHashFullName))
                            continue;

                        var filesNamesGrouped = filesHashGrouped
                            .Where(fh => fh.File.GetFullName().ToLower() != fileHashFullName.ToLower())
                            .GroupBy(fhg => fhg.File.GetFullName())
                            .Select(fhg => fhg.Key)
                            .OrderBy(n=>n)
                            .ToArray();

                        if (!filesNamesGrouped.Any())
                            break;

                        var oldFileFullPathAndName = fileHash.File.GetFullPathAndName();
                        System.Diagnostics.Debug.WriteLine(oldFileFullPathAndName);

                        if (!System.IO.File.Exists(oldFileFullPathAndName))
                            continue;

                        var oldFileFullPath = fileHash.File.GetFullPath();
                        var startPath = oldFileFullPath.Length <= 50 ? 0 : oldFileFullPath.Length - 50;

                        Console.Clear();

                        Console.WriteLine($"--> {fileHashFullName} - {oldFileFullPath.Substring(startPath)}");

                        for (var x = 0; x <= filesNamesGrouped.Length - 1; x++)
                        {
                            var firstPath = filesHashGrouped.Where(fhg => fhg.File.GetFullName().ToLower() == filesNamesGrouped[x].ToLower())
                                .First().File.GetFullPath();
                            var startPath2 = firstPath.Length <= 50 ? 0 : firstPath.Length - 50;
                            Console.WriteLine($"[{x}] {filesNamesGrouped[x]} - {firstPath.Substring(startPath2)}");
                        }

                        Console.WriteLine();
                        Console.WriteLine(string.Empty);
                        Console.WriteLine("[I]gnore, [S]kip, Ignore [a]ll or number: ");

                        var keyInfo = Console.ReadKey();
                        var key = keyInfo.KeyChar.ToString().ToLower();
                        var exitLoop = false;

                        switch (key)
                        {
                            case "s":
                                continue;
                            case "a":
                                exitLoop = true;
                                break;
                            case "i":
                                ignoreNames.Add(fileHashFullName);
                                break;
                            default:
                                if (!int.TryParse(key, out int intOpt))
                                    intOpt = -1;

                                if (intOpt < 0 || intOpt > filesNamesGrouped.Length - 1)
                                {
                                    Console.WriteLine("Invalid option");
                                    Console.ReadKey();
                                    break;
                                }

                                var newName = filesNamesGrouped[intOpt];
                                var newPath = string.Copy(oldFileFullPath);

                                if (System.IO.File.Exists($"{newPath}\\{newName}"))
                                {
                                    newPath = $"{newPath}\\#old";

                                    if (!System.IO.Directory.Exists(newPath))
                                        System.IO.Directory.CreateDirectory(newPath);

                                    if (System.IO.File.Exists($"{newPath}\\{newName}"))
                                    {
                                        if (System.IO.Directory.Exists(newPath + "\\1") && !System.IO.File.Exists(newPath + "\\1\\" + newName))
                                            newPath = newPath + "\\1";
                                        else if (System.IO.Directory.Exists(newPath + "\\2") && !System.IO.File.Exists(newPath + "\\2\\" + newName))
                                            newPath = newPath + "\\2";
                                        else if (System.IO.Directory.Exists(newPath + "\\3") && !System.IO.File.Exists(newPath + "\\3\\" + newName))
                                            newPath = newPath + "\\3";
                                        else if (System.IO.Directory.Exists(newPath + "\\4") && !System.IO.File.Exists(newPath + "\\4\\" + newName))
                                            newPath = newPath + "\\4";
                                        else if (System.IO.Directory.Exists(newPath + "\\5") && !System.IO.File.Exists(newPath + "\\5\\" + newName))
                                            newPath = newPath + "\\5";
                                        else if (System.IO.Directory.Exists(newPath + "\\6") && !System.IO.File.Exists(newPath + "\\6\\" + newName))
                                            newPath = newPath + "\\6";
                                        else if (!System.IO.Directory.Exists(newPath + "\\1"))
                                        {
                                            newPath = newPath + "\\1";
                                            System.IO.Directory.CreateDirectory(newPath);
                                        }
                                        else if (!System.IO.Directory.Exists(newPath + "\\2"))
                                        {
                                            newPath = newPath + "\\2";
                                            System.IO.Directory.CreateDirectory(newPath);
                                        }
                                        else if (!System.IO.Directory.Exists(newPath + "\\3"))
                                        {
                                            newPath = newPath + "\\3";
                                            System.IO.Directory.CreateDirectory(newPath);
                                        }
                                        else if (!System.IO.Directory.Exists(newPath + "\\4"))
                                        {
                                            newPath = newPath + "\\4";
                                            System.IO.Directory.CreateDirectory(newPath);
                                        }
                                        else if (!System.IO.Directory.Exists(newPath + "\\5"))
                                        {
                                            newPath = newPath + "\\5";
                                            System.IO.Directory.CreateDirectory(newPath);
                                        }
                                        else if (!System.IO.Directory.Exists(newPath + "\\6"))
                                        {
                                            newPath = newPath + "\\6";
                                            System.IO.Directory.CreateDirectory(newPath);
                                        }
                                    }
                                }

                                System.IO.File.SetAttributes(oldFileFullPathAndName, ~System.IO.FileAttributes.ReadOnly);
                                System.IO.File.Move(oldFileFullPathAndName, $"{newPath}\\{newName}");
                                break;
                        }

                        if (exitLoop)
                            break;
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            //deleteThumbs();

            //M4();

            //while (Reload())
            //{ }

            //AllFolder2();
            //AllFolder3();
            //AllOldFolder4();
            //AllDownloadsRealPlayerFolder4();
            //AllCloudFolder4();

            //AllArrumarFolder4();
            //AllEmptyFolders();

            //AllEmptyFolders();
            //FixFileNames();


        }
    }
}
