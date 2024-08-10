using System;

namespace F8Framework.Core
{
    public class LayerNotify : LayerUI
    {
        public string Show(int uiId, UIConfig config, string content, UICallbacks callbacks = null)
        {
            var prefabPath = config.AssetName;
            ViewParams viewParams;
            string guid = Guid.NewGuid().ToString(); // 生成一个唯一的ID
            
            if (!uiViews.TryGetValue(prefabPath, out viewParams))
            {
                if (!uiCache.TryGetValue(prefabPath, out viewParams))
                {
                    viewParams = new ViewParams();
                    viewParams.Guid = guid;
                    viewParams.PrefabPath = prefabPath;
                    uiViews.Add(viewParams.Guid, viewParams);
                }
                else
                {
                    viewParams.Guid = guid;
                    viewParams.PrefabPath = prefabPath;
                    uiViews.Add(viewParams.Guid, viewParams);
                }
            }

            viewParams.UIid = uiId;
            viewParams.Params = new object[] { content };
            viewParams.Callbacks = callbacks;
            viewParams.Valid = true;
            
            Load(viewParams);
            return guid;
        }
        
        public int CloseByGuid(string guid, bool isDestroy)
        {
            if (uiViews.TryGetValue(guid, out var viewParams))
            {
                if (isDestroy)
                {
                    RemoveCache(viewParams.PrefabPath);
                }
                else
                {
                    uiCache[viewParams.PrefabPath] = viewParams;
                }
                var comp = viewParams.DelegateComponent;
                comp.Remove(isDestroy);
                viewParams.Valid = false;
                return viewParams.UIid;
            }

            return default;
        }
    }
}