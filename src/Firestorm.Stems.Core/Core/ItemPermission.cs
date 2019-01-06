using System;

namespace Firestorm.Stems
{
    [Flags]
    public enum ItemPermission
    {
        None = 0,
        Read = 1,
        Write = 2,
        ReadWrite = Read | Write,
        Delete = 4,
        ReadWriteDelete = ReadWrite | Delete
    }
}