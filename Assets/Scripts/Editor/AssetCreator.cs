using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.IO;
using UnityEditor;
using UnityEngine;
using ObjectFieldAlignment = Sirenix.OdinInspector.ObjectFieldAlignment;

public class AssetCreator : OdinMenuEditorWindow
{
    public static string itemsPath = "Assets/Items/Items/";
    public static string editorPath = "Assets/Editor/";

    [MenuItem("Tools/Hydroxia Asset Creator")]
    private static void OpenWindow()
    {
        GetWindow<AssetCreator>().Show();
    }

    public enum ItemType { Tools, Placeables, Nature, Craftables, Stations, Armor }
    public float item;
    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        tree.Add("Create New Item", new CreateItem());

        tree.AddAllAssetsAtPath("Tools", itemsPath + "Tool", typeof(SO_Item), true, true);
        tree.AddAllAssetsAtPath("Armor", itemsPath + "Armor", typeof(SO_Item), true, true);
        tree.AddAllAssetsAtPath("Placeables", itemsPath + "Placeable", typeof(SO_Item), true, true);
        tree.AddAllAssetsAtPath("Nature", itemsPath + "Nature", typeof(SO_Item), true, true);
        tree.AddAllAssetsAtPath("Craftables", itemsPath + "Craftable", typeof(SO_Item), true, true);
        tree.AddAllAssetsAtPath("Stations", itemsPath + "Station", typeof(SO_Item), true, true);

        return tree;
    }

    private class CreateItem
    {
        #region PARAMETERS
        private bool objectMenuActive;
        [HorizontalGroup("top"), VerticalGroup("top/left"), PreviewField(Alignment = ObjectFieldAlignment.Left, Height = 90), InlineEditor(DrawGUI = false, DrawPreview = true, ObjectFieldMode = InlineEditorObjectFieldModes.CompletelyHidden)]
        [ShowIf("@objectMenuActive == true"), SerializeField]
        private Sprite sprite;

        [HorizontalGroup("top")]
        [ShowIf("@objectMenuActive == true"), SerializeField, AssetsOnly, VerticalGroup("top/right")]
        private GameObject itemModel;
        Camera cam;
        #endregion
        #region METHODS
        [ShowIf("@objectMenuActive == false"), Button]
        void Create()
        {
            // BRING UP MENU
            objectMenuActive = true;
        }

        [ShowIf("@objectMenuActive == true && itemModel != null"), Button(DisplayParameters = true), VerticalGroup("top/right")]
        void CreateIcon(int iconWidth = 128, int iconHeight = 128, DefalutCameraPosition cameraPosition = DefalutCameraPosition.forward, CameraOffset cameraOffset = CameraOffset.topRight, float distance = 2)
        {
            // create place camera 
            LocRot transform = IconCamera.CalculatePosition(cameraPosition, cameraOffset, distance);
            cam = new GameObject("Asset Creator Camera").AddComponent<Camera>();
            cam.transform.position = transform.location;
            cam.transform.rotation = transform.rotation;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.black;
            cam.cullingMask = LayerMask.GetMask("IconRendering") << 0;
            Selection.activeObject = cam.transform;
            // create and place item
            GameObject item = Instantiate(itemModel);
            item.layer = LayerMask.GetMask("IconRendering");
            // create icon
            RenderTexture renderTexture = cam.activeTexture;
            Texture2D texture = new Texture2D(iconWidth, iconHeight);
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();
        }

        [ShowIf("@objectMenuActive == true"), EnableIf("@itemModel"), Button, VerticalGroup("top/right")]
        void FinalizeAsset()
        {
            // CREATE ASSET 
            Debug.Log("Created Asset");
        }

        [ShowIf("@objectMenuActive == true"), Button, VerticalGroup("top/right")]
        void Cancel()
        {
            // CLOSE MENU
            objectMenuActive = false;
            if (cam) DestroyImmediate(cam.gameObject);
        }
        #endregion
    }
}
#region   ICON CREATION
public enum DefalutCameraPosition { up, down, left, right, forward, back }
public enum CameraOffset { left, right, up, down, topLeft, topRight, bottomLeft, bottomRight }
public class IconCamera
{
    public static LocRot CalculatePosition(DefalutCameraPosition cameraPosition, CameraOffset cameraOffset, float distance)
    {
        LocRot transform = new LocRot();

        switch (cameraPosition)
        {
            case DefalutCameraPosition.up:
                transform.location = Vector3.up * distance;
                break;
            case DefalutCameraPosition.down:
                transform.location = Vector3.down * distance;
                break;
            case DefalutCameraPosition.left:
                transform.location = Vector3.left * distance;
                break;
            case DefalutCameraPosition.right:
                transform.location = Vector3.right * distance;
                break;
            case DefalutCameraPosition.forward:
                transform.location = Vector3.forward * distance;
                break;
            case DefalutCameraPosition.back:
                transform.location = Vector3.back * distance;
                break;
            default:
                break;
        }

        switch (cameraOffset)
        {
            case CameraOffset.left:
                transform.location += Vector3.left / 2 * distance;
                break;
            case CameraOffset.right:
                transform.location += Vector3.right / 2 * distance;
                break;
            case CameraOffset.up:
                transform.location += Vector3.up / 2 * distance;
                break;
            case CameraOffset.down:
                transform.location += Vector3.down / 2 * distance;
                break;
            case CameraOffset.topLeft:
                transform.location += (Vector3.left + Vector3.up) / 2 * distance;
                break;
            case CameraOffset.topRight:
                transform.location += (Vector3.right + Vector3.up) / 2 * distance;
                break;
            case CameraOffset.bottomLeft:
                transform.location += (Vector3.left + Vector3.down) / 2 * distance;
                break;
            case CameraOffset.bottomRight:
                transform.location += (Vector3.right + Vector3.down) / 2 * distance;
                break;
            default:
                break;
        }

        transform.rotation = Quaternion.LookRotation(-transform.location);

        return transform;
    }
}
public class LocRot
{
    public Vector3 location;
    public Quaternion rotation;
}
#endregion