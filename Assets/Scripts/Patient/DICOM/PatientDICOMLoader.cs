﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using itk.simple;


/*! This class is respondible for parsing DICOM directories and loading the images.
 * When events happen, other modules are notified using the PatientEventSystem class. */
public class PatientDICOMLoader
{
	//! DICOM loader instance:
	private static DicomLoaderITK mDicomLoader = new DicomLoaderITK ();

	//! Available series found in the folder:
	private static VectorString mAvailableSeries = new VectorString ();

	//! Already loaded DICOM:
	private static DICOM mCurrentDICOM = null;

	//! Path which should be searched:
	private static string mPath = "";

	//! Simple lock, used to prevent loading multiple directory or DICOMs at the same time:
	private static bool isLoading = false;

    private static int DicomIDForThread = 0;


	public static void loadDirectory( string path )
	{
		if (!isLoading) {

			// Lock:
			isLoading = true;

			mPath = path;
			// Parse the directory:
			mAvailableSeries = mDicomLoader.loadDirectory (path);
			PatientEventSystem.triggerEvent (PatientEventSystem.Event.DICOM_NewList);

			// Unlock:
			isLoading = false;

			loadDicom (0);
		}
	}

	public static void loadDicom( int id )
	{
		if (!isLoading) {
			// Lock:
			isLoading = true;

            /*DicomIDForThread = id;

            ThreadUtil t = new ThreadUtil(loadDicomWorker, loadDicomCallback);
            t.Run();	*/

            //Dont work, because mDicomLoader.load() creates a untiy game object TODO
            
             



            // If there was a series found with the given ID, laod it:
            if (mAvailableSeries.Count > DicomIDForThread)
            {
                mCurrentDICOM = mDicomLoader.load(mPath, mAvailableSeries[DicomIDForThread]);
            }

            if (mCurrentDICOM != null)
            {
                PatientEventSystem.triggerEvent(PatientEventSystem.Event.DICOM_NewLoaded);
            }
            // Unlock:
            isLoading = false;

        }
	}

    private static void loadDicomWorker(object sender, DoWorkEventArgs e)
    {
        // If there was a series found with the given ID, laod it:
        if (mAvailableSeries.Count > DicomIDForThread)
        {
            mCurrentDICOM = mDicomLoader.load(mPath, mAvailableSeries[DicomIDForThread]);
        }
    }

    private static void loadDicomCallback(object sender, RunWorkerCompletedEventArgs e)
    {
        // If a series was loaded successfully, let listeners know:
        if (mCurrentDICOM != null)
        {
            PatientEventSystem.triggerEvent(PatientEventSystem.Event.DICOM_NewLoaded);
        }
        // Unlock:
        isLoading = false;
    }



    public static List<string> getAvailableSeries()
	{
		List<string> list = new List<string>();
		foreach( string s in mAvailableSeries )
		{
			list.Add (s);
		}
		return list;
	}

	public static DICOM getCurrentDicom()
	{
		return mCurrentDICOM;
	}
}

