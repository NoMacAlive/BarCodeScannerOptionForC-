using System;
using System.ComponentModel;
using System.Drawing;
using Newtonsoft.Json;
using Accusoft.BarcodeXpressSdk;
using Aspose.BarCode.BarCodeRecognition;
using asprise_ocr_api;
using IronBarCode;

namespace BarcodePoC
{
	class Program
	{
		static void Main( string[ ] args )
		{
			foreach ( FileName fileName in Enum.GetValues( typeof( FileName ) ) )
			{
				Console.WriteLine( fileName.GetDescription( ) + " Results:" );
				Console.WriteLine( "**************************************" );
				ScanTheProvidedFile( fileName );
			}
		}

		private static void ScanTheProvidedFile( FileName fileName )
		{
			// // string testFileName = "test-barcodes.bmp";
			string testFileName = fileName.GetDescription( );
			try
			{
				SpireScan( testFileName );
			}
			catch ( Exception e )
			{
				Console.WriteLine( e.Message );
			}

			try
			{
				BarCodeXpressScan( testFileName );
			}
			catch ( Exception e ) { Console.WriteLine( e.Message ); }

			try
			{
				IronBarCodeScan( testFileName );
			}
			catch ( Exception e ) { Console.WriteLine( e.Message ); }

			try
			{
				AspriseOCRScan( testFileName );
			}
			catch ( Exception e ) { Console.WriteLine( e.Message ); }

			try
			{
				AsposeScan( testFileName );
			}
			catch ( Exception e ) { Console.WriteLine( e.Message ); }
		}

		private static void AsposeScan( string testFileName )
		{
			///
			/// This is the Aspose barCodeScanner
			///
			using ( BarCodeReader reader = new BarCodeReader( testFileName ) )

			{
				Console.WriteLine( "Aspose Results:" );
				foreach ( BarCodeResult result in reader.ReadBarCodes( ) )

				{
					Console.WriteLine( "Type: " + result.CodeType );

					Console.WriteLine( "CodeText: " + result.CodeText );
				}
			}

			Console.WriteLine( "======================" );
		}

		private static void AspriseOCRScan( string testFileName )
		{
			///
			/// This is the AspriseOCR barCodeScanner
			/// 
			AspriseOCR.SetUp( );
			AspriseOCR ocr = new AspriseOCR( );
			ocr.StartEngine( "eng", AspriseOCR.SPEED_FASTEST );

			string s = ocr.Recognize( testFileName, -1, -1, -1, -1, -1, AspriseOCR.RECOGNIZE_TYPE_ALL, AspriseOCR.OUTPUT_FORMAT_PLAINTEXT );
			Console.WriteLine( "AspriseOCR Results:" );
			Console.WriteLine( JsonConvert.SerializeObject( s, Formatting.Indented ) );
			Console.WriteLine( "======================" );

			ocr.StopEngine( );
		}

		private static void IronBarCodeScan( string testFileName )
		{
			///
			/// This is the IronBarcode barCodeScanner
			/// 
			BarcodeResult Result = BarcodeReader.QuicklyReadOneBarcode( testFileName, BarcodeEncoding.All, true );
			Console.WriteLine( "IronBarcode Results:" );
			Console.WriteLine( JsonConvert.SerializeObject( Result, Formatting.Indented ) );
			Console.WriteLine( "======================" );
		}

		private static void BarCodeXpressScan( string testFileName )
		{
			///
			/// This is the Xpress barCodeScanner
			/// 
			using ( BarcodeXpress barcodeXpress = new BarcodeXpress( "." ) )
			using ( Bitmap bitmap = new Bitmap( testFileName ) )
			{
				barcodeXpress.reader.BarcodeTypes = new BarcodeType[ ]
				{
					BarcodeType.Code39Barcode,
					BarcodeType.IntelligentMailBarcode,
					BarcodeType.Code128Barcode,
					BarcodeType.DataMatrixBarcode,
					BarcodeType.QRCodeBarcode,
					BarcodeType.CodabarBarcode,
					BarcodeType.UPCABarcode
				};
				Accusoft.BarcodeXpressSdk.Result[ ] results = barcodeXpress.reader.Analyze( bitmap );

				Console.WriteLine( "BarCode Xpress Results:" );
				Console.WriteLine( JsonConvert.SerializeObject( results, Formatting.Indented ) );
				Console.WriteLine( "======================" );
			}
		}

		private static void SpireScan( string testFileName )
		{
			///
			///This is the Apire.Barcode scanner
			/// 
			// string[] datas = Spire.Barcode.BarcodeScanner.Scan("sampleAustraliaWithBarcode.jpg");
			string[ ] datas = Spire.Barcode.BarcodeScanner.Scan( testFileName );
			if ( datas.Length > 0 )
			{
				Console.WriteLine( "Spire Barcode Result:" );
				Console.WriteLine( JsonConvert.SerializeObject( datas, Formatting.Indented ) );
				Console.WriteLine( "======================" );
			}
		}
	}

	public enum FileName
	{
		[Description( "Resources/pdfs/cameron.pdf" )]
		CameronPDF = 0,

		[Description( "Resources/pdfs/ElectricityBill.pdf" )]
		ElectricityBillPDF,

		[Description( "Resources/pdfs/GasBill.pdf" )]
		GasBillPDF,

		[Description( "Resources/pdfs/newPlymouth.pdf" )]
		NewPlymouthPDF,

		[Description( "Resources/images/cameron_page-0001.jpg" )]
		CameronIMG1,

		[Description( "Resources/images/cameron_page-0002.jpg" )]
		CameronIMG2,

		[Description( "Resources/images/ElectricityBill.jpg" )]
		ElectricityBillIMG,

		[Description( "Resources/images/GasBill.jpg" )]
		GasBillIMG,

		[Description( "Resources/images/newPlymouth.jpg" )]
		NewPlymouthIMG
	}
}