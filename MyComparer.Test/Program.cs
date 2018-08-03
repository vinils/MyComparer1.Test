namespace MyComparer.Test
{
    using MyComparer.Hashers.MD5s;
    using System;
    using System.Collections.Generic;

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
            var directoryInfo = new System.IO.DirectoryInfo("U:\\Cz\\a\\BackUp");
            global::MyList.DAL.Entities.Directory directory = directoryInfo;
            var guid = Guid.NewGuid();
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
            var guid = new Guid("c15a50ba-e925-45c2-9468-e17c68ff8ff0");

            using (var sql = new Hashers.DAL.Clouds.FileHash(guid))
            {
                var memory = new List<Hashers.DAL.Entities.FileHash>(sql);
                if (memory.Count >= 2093396)
                    return false;
            }

            try
            {
                var directoryInfo = new System.IO.DirectoryInfo("U:\\");
                global::MyList.DAL.Entities.Directory directory = directoryInfo;
                using (var myEqualityComparer = new Hashers.MD5s.Cloud(guid))
                {
                    myEqualityComparer.ReloadFiles();
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

        static void Main(string[] args)
        {
            var hasToReload = true;
            do
            {
                hasToReload = Reload();
            } while (hasToReload);
        }
    }
}
