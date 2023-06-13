using System;

namespace Sequences
{
    [Serializable] public class ActiveSection : Section { }
    [Serializable] public class ActiveSection<T> : Section<T> { }
    [Serializable] public class CancelSection : Section { }
    [Serializable] public class CompleteSection : Section { }
    [Serializable] public class EndSection : Section { }
}
