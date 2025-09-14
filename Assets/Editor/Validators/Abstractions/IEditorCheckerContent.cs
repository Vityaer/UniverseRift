using Cysharp.Threading.Tasks;

namespace Editor.Common.Validators.Abstractions
{
    public interface IEditorCheckerContent
    {
        UniTask Check();
    }
}