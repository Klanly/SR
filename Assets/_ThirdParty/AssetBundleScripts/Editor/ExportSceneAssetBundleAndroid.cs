//using UnityEngine;
//using System.Collections;
//
//public class ExportSceneAssetBundleAndroid : MonoBehaviour {
//
// [MenuItem("Assets/Build AssetBundle From Selection - Track dependencies Android")]
//        static void ExportResource () {
//            // Bring up save panel
//            string path = EditorUtility.SaveFilePanel ("Save Resource", "", "bundle_android", "unity3d");
//            if (path.Length != 0) {
//                // Build the resource file from the active selection.
//                Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
//                BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,BuildTarget.Android);
//                Selection.objects = selection;
//            }
//        }
//        [MenuItem("Assets/Build AssetBundle From Selection - No dependency tracking Android")]
//        static void ExportResourceNoTrack () {
//            // Bring up save panel
//            string path = EditorUtility.SaveFilePanel ("Save Resource", "", "bundle_android", "unity3d");
//            if (path.Length != 0) {
//                // Build the resource file from the active selection.
//                BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path);
//            }
//        }
//}
