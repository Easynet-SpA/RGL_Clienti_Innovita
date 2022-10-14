using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InnovitaEtichette
{

    /**
     * 
     */
    public class Articolo
    {

        /**
         * 
         */
        public static void CartellinoFinito(string stampante, List<InnovitaBarcodes.ArticoloBarcode> articoli)
        {

            if (articoli.Count == 0) return;

            var rs = new Rgl.SBOBaseLight.SBORecordset();


            // Report per Zebra
            foreach (var articolo in articoli)
            {
                rs.DoQuery(string.Format("SELECT   T0.\"U_RGL_CM_Etc\" " +
                                         " FROM RGL_CM_OITM AS T0 " +
                                         " WHERE T0.\"ItemCode\" = '{0}'  AND T0.\"U_RGL_PUR_Etc\" <> ''", articolo.ItemCode));
                if (rs.Eof) return;

                var basePath = System.IO.Path.Combine(Rgl.SBOBaseLight.SAPConnector.Company.WordDocsPath, "Layout");

                var report = System.IO.Path.Combine(basePath, rs.AsString("U_RGL_CM_Etc"));


                rs.DoQuery($"SELECT   T0.\"U_Larghezza\", T0.\"U_Lunghezza\", T0.\"U_Diametro\", T0.\"U_Spessore\", T2.\"CardCode\", T0.\"U_RGL_CM_QtyNR\", T0.\"U_RGL_CM_QtyTO\" " +
                           $"       , T0.\"U_Qualita\", T0.\"ItemCode\", T0.\"InDate\", T0.\"DistNumber\", T3.\"U_SpessoreUNI\", T3.\"U_DiametroUNI\" " +
                           $" FROM RGL_CM_OBTN AS T0" +
                           $"      LEFT OUTER JOIN RDR1 AS T1 ON T0.\"U_RGL_CM_Prd\" = T1.\"DocEntry\"" +
                           $"      LEFT OUTER JOIN ORDR AS T2 ON T2.\"DocEntry\" = T1.\"DocEntry\" " +
                           $"      LEFT OUTER JOIN \"@RGL_CM_PRD_T\" AS T3 ON T3.\"DocEntry\" = T0.\"U_RGL_CM_Prd\" " +
                           $" WHERE T0.\"ItemCode\" = '{articolo.ItemCode}' AND T0.\"DistNumber\" = '{articolo.DistNumber}'");

                for (int i = 0; i < 1; i++)
                {
                    var out_zpl = System.IO.File.ReadAllText(report);

                    out_zpl = out_zpl.Replace("@CUSTOMERCODE@", rs.AsString("CardCode"));
                    out_zpl = out_zpl.Replace("@DIMENSION@", $"{rs.AsDouble("U_Diametro"):#.##}x{rs.AsDouble("U_Spessore"):#.##}x{rs.AsDouble("U_Lunghezza"):#.##}");
                    out_zpl = out_zpl.Replace("@QTY@", $"{rs.AsDouble("U_RGL_CM_QtyNR"):#}");
                    out_zpl = out_zpl.Replace("@QTY2@", $"{rs.AsDouble("U_RGL_CM_QtyTO") * 1000.0}");
                    out_zpl = out_zpl.Replace("@RAWMATERIALCODE@", $"{rs.AsString("U_Qualita")}");
                    out_zpl = out_zpl.Replace("@RAWMATERIALITEMCODE@", $"{rs.AsString("ItemCode")}");
                    out_zpl = out_zpl.Replace("@NORMA@", $"{rs.AsString("U_SpessoreUNI")}");
                    out_zpl = out_zpl.Replace("@USER@", "SP");
                    out_zpl = out_zpl.Replace("@DATE@", $"{rs.AsDate("InDate"):dd/MM/yyyy}");
                    out_zpl = out_zpl.Replace("@BARCODE@", $"{rs.AsString("DistNumber")}");

                    Zebra_Send(stampante, out_zpl);
                }
            }

        }

        #region "Zebra"

        /**
         * 
         */
        private static void Zebra_Send(string ipAddress, string ZPLString)
        {
            // Printer IP Address and communication port
            int port = 9100;

            try
            {
                // Open connection
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
                client.Connect(ipAddress, port);

                // Write ZPL String to connection
                System.IO.StreamWriter writer = new System.IO.StreamWriter(client.GetStream());
                writer.Write(ZPLString);
                writer.Flush();

                // Close Connection
                writer.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }

}
