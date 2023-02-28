using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEditor.Build.Reporting;
using System.IO;
using UnityEngine;

class BuildScript {
	static string[] SCENES = FindEnabledEditorScenes();

	static string APP_NAME = "Namer";
	static string TARGET_DIR = "../SlackPost";

	static void PerformAllBuilds ()
	{
		PerformMacOSXBuild ();
		PerformWindowsBuild ();
		PerformWebGLBuild ();
		PerformAndroidBuild();
		PerformiOSBuild();
	}

	static void PerformMacOSXBuild ()
	{
		string target_path = "/Mac/";
		string target_exec = APP_NAME;
		GenericBuild(SCENES, TARGET_DIR + target_path, target_exec, BuildTarget.StandaloneOSX,BuildOptions.None);
	}

	static void PerformWindowsBuild ()
	{
		string target_path = "/Windows/";
		string target_exec = APP_NAME + ".exe";
		GenericBuild(SCENES, TARGET_DIR + target_path, target_exec,  BuildTarget.StandaloneWindows,BuildOptions.None);
	}
	
	static void PerformWebGLBuild ()
	{
		string target_dir = "/webgl/";
		GenericBuild(SCENES, TARGET_DIR + target_dir, "", BuildTarget.WebGL,BuildOptions.None);
	}

	static void PerformAndroidBuild ()
	{
        PlayerSettings.Android.keystorePass = "chemistry123";
        PlayerSettings.Android.keyaliasName = "namer";
        PlayerSettings.Android.keyaliasPass = "namer123";

        string target_dir = "/Android/";
		string target_exec = APP_NAME + ".apk";
		GenericBuild(SCENES, TARGET_DIR + target_dir, target_exec, BuildTarget.Android,BuildOptions.None);
	}

	static void PerformiOSBuild ()
	{
		string target_path = "/IOS/";
		//We do not build the xcodeproject in the target directory, since we do not want to archive the
		//entire xcode project, but instead build with XCode, then output the .ipa through Jenkins
		GenericBuild(SCENES, TARGET_DIR + target_path, "",BuildTarget.iOS,BuildOptions.None);
	}

	private static string[] FindEnabledEditorScenes() {
		List<string> EditorScenes = new List<string>();
		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
			if (!scene.enabled) continue;
			EditorScenes.Add(scene.path);
		}
		return EditorScenes.ToArray();
	}

	static void GenericBuild(string[] scenes, string target_dir, string target_exec,  BuildTarget build_target, BuildOptions build_options)
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
		var res = BuildPipeline.BuildPlayer(scenes, target_dir+target_exec, build_target, build_options);
		if (res.summary.result == BuildResult.Succeeded)
		{
			ZipDirectory (Path.GetDirectoryName(Application.dataPath) + "/" + target_dir, build_target);
		}
		else {
			throw new Exception("BuildPlayer failure: " + res);
		}
	}
	
	static bool ZipDirectory(string directoryPath, BuildTarget build_target)
	{
		var outputZipPath = directoryPath + "../" + build_target.ToString() + ".zip";	
    		
        	if (File.Exists(outputZipPath))
        	{
            	    File.Delete(outputZipPath);
        	}
        	ZipFile.CreateFromDirectory(directoryPath, outputZipPath);
                
        	return true;
	}
}