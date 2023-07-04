using System.Collections.Generic;

namespace PlatformLib.ui.framework.ability
{
    public interface IDualInsertionStyle<T>
    {
        int InsertFront(List<T> list);
        int InsertBack(List<T> list);
    }
}
