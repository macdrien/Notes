using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Listes de fonctions utiles à la lecture de fichiers excels
/// </summary>
/// 
/// <remarks>
/// Les classes et fonctions qui suivent utilisent le paquetage NuGet DocumentFormat.OpenXml
/// </remarks>
namespace GestionDocuments
{
    class Program
    {
        static void Main(string[] args)
        {
            const string CHEMIN_FICHIER = "C:\\CYDev\\Workspace\\tuto_c_sharp\\GestionDocuments\\doc_test\\",
                         NOM_FICHIER    = "feuille_calcul_test.xlsx",
                         FEUILLE        = "Feuil1";

            /* --- Différente lecture --- */
            Console.WriteLine("Appel de la lecture par cellule");
            ReadFileByCells(CHEMIN_FICHIER + NOM_FICHIER);
            //Console.WriteLine("Appel de la lecture quadrillée : ");
            //ReadFileByRowsAndCells(CHEMIN_FICHIER + NOM_FICHIER);
            /*Console.Write("\nAppuyez sur entrée pour tester la fonction : ");
            Console.ReadLine();*/

            //AutomatisationRapport.ReadRapport(CHEMIN_FICHIER + NOM_FICHIER);
            /* --- Tests de modifications --- */
            //IncrementNumbers(CHEMIN_FICHIER + NOM_FICHIER);
            //TestToPlaceAStringInValue(CHEMIN_FICHIER + NOM_FICHIER);
            #region appel de la fonction GetSpreadsheetCell
            /*using (SpreadsheetDocument doc = SpreadsheetDocument.Open(CHEMIN_FICHIER + NOM_FICHIER, false))
            {
                Sheet sheet = doc.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>().First<Sheet>();
                WorksheetPart worksheetPart = (WorksheetPart)doc.WorkbookPart.GetPartById(sheet.Id.Value);
                Console.WriteLine("C2 : " + GetSpreadsheetCell(worksheetPart.Worksheet, "C", 2).CellValue.Text);
            }*/
            #endregion
            //RemoveCell(CHEMIN_FICHIER + NOM_FICHIER, "Feuil1", "C", 2);
            //CreateWorkbook(CHEMIN_FICHIER + "doc_test_creation.xlsx", "Feuille");
            //Console.WriteLine(ChangeStringValueInCell(CHEMIN_FICHIER + NOM_FICHIER, "Feuil1", "B", 1, "Clemessy") ? "Changement fait\n" : "Erreur lors du changement\n");
            //ReplaceAStringValueByAnInt(CHEMIN_FICHIER + NOM_FICHIER, FEUILLE, "A", 1, 34);

            /*Console.WriteLine("Appel de la lecture quadrillée après modifications: ");
            ReadFileByRowsAndCells(CHEMIN_FICHIER + NOM_FICHIER);*/

            Console.ReadLine();
        }

        /// <summary>
        /// Lit et affiche le contenu d'un fichier excel cellule par cellule.
        /// </summary>
        /// 
        /// <param name="pathToFile">
        /// Le chemin (nom et extension compris) du fichier à lire
        /// </param>
        private static void ReadFileByCells(String pathToFile)
        {

            Console.WriteLine("Fichier : " + pathToFile);

            // Ouverture du document
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(pathToFile, false))
            {
                // Parcours de la hiérarchie du document jusqu'à récupération du tableau
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                // Récupération du tableau nommé "Feuil1"
                Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().Where(x => x.Name == "Feuil1").FirstOrDefault<Sheet>();
                WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                // Récupération de la liste des string liées
                SharedStringTable listeStringElement = workbookPart.SharedStringTablePart.SharedStringTable;
                // Récupération de la liste des cellules
                IEnumerable<Cell> listCell = worksheetPart.Worksheet.Descendants<Cell>().Where(x => x.CellReference != null);

                foreach (Cell c in listCell)
                {
                    // Suppression des référencées mais vides
                    if (c.CellValue == null)
                    {
                        continue;
                    }

                    // Test si la cellule est de type string
                    if (c.DataType == null || !(c.DataType.InnerText == "s"))
                    { // Si non on affiche directement son contenu
                        Console.WriteLine(c.CellReference.Value + " : " + c.CellValue.InnerText);
                    }
                    else
                    { // Si oui on affiche la string qui y est associée dans la liste des string (via l'id dans la liste)
                        Console.WriteLine(c.CellReference.Value + " : " +
                            listeStringElement.ElementAt(Int32.Parse(c.CellValue.InnerText)).InnerText);
                    }
                }
                spreadsheetDocument.Close();
            }
        }

        /// <summary>
        /// Lit et affiche le contenu d'un fichier excel ligne
        ///     par ligne et cellule par cellule.
        /// </summary>
        /// 
        /// <param name="pathToFile">
        /// Le chemin (nom et extension compris) du fichier à lire
        /// </param>
        private static void ReadFileByRowsAndCells(String pathToFile)
        {
            Console.WriteLine("Fichier : " + pathToFile);

            // Ouverture du document
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(pathToFile, false))
            {
                // Parcours de la hiérarchie du document jusqu'à récupération du tableau
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                // Récupération du tableau nommé "Feuil1"
                Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().Where(x => x.Name == "Feuil1").FirstOrDefault<Sheet>();
                WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                // Récupération de la liste des string liées
                SharedStringTable listeStringElement = workbookPart.SharedStringTablePart.SharedStringTable;
                // Récupération de la liste des cellules
                IEnumerable<Row> listRow = worksheetPart.Worksheet.Descendants<Row>().Where(x => x.RowIndex != null);

                foreach (Row ligne in listRow)
                {
                    IEnumerable<Cell> listCell = ligne.Descendants<Cell>();
                    foreach (Cell c in listCell)
                    {
                        // Suppression des référencées mais vides
                        if (c.CellValue == null)
                        {
                            continue;
                        }

                        // Test si la cellule est de type string
                        if (c.DataType == null || c.DataType.InnerText != "s")
                        { // Si non on affiche directement son contenu
                            Console.Write(c.CellValue.InnerText + "  ");
                        }
                        else
                        { // Si oui on affiche la string qui y est associée dans la liste des string (via l'id dans la liste)
                            Console.Write(listeStringElement.ElementAt(Int32.Parse(c.CellValue.InnerText)).InnerText + " ");
                        }
                    }
                    Console.WriteLine();
                }
                spreadsheetDocument.Close();
            }
        }

        /// <summary>
        /// Lit cellule par cellule un fichier excel.
        /// Si le lecteur rencontre un nombre alors ce nombre est incrémenté.
        /// </summary>
        /// 
        /// <param name="pathToFile">
        /// Le chemin (nom et extension compris) du fichier à lire et modifier
        /// </param>
        private static void IncrementNumbers(String pathToFile)
        {
            Console.WriteLine("Fichier : " + pathToFile);

            // Ouverture du document
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(pathToFile, true))
            {
                // Parcours de la hiérarchie du document jusqu'à récupération du tableau
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                // Récupération du tableau nommé "Feuil1"
                Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().Where(x => x.Name == "Feuil1").FirstOrDefault<Sheet>();
                WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                // Récupération de la liste des string liées
                SharedStringTable listeStringElement = workbookPart.SharedStringTablePart.SharedStringTable;
                // Récupération de la liste des cellules
                IEnumerable<Cell> listCell = worksheetPart.Worksheet.Descendants<Cell>().Where(x => x.CellReference != null);

                foreach (Cell c in listCell)
                {
                    // Suppression des référencées mais vides
                    if (c.CellValue == null)
                    {
                        continue;
                    }

                    // Test si la cellule est un entier et ne contient pas de formule
                    if (c.DataType == null && c.CellFormula == null)
                    { // Alors on incrémente la valeur
                        c.CellValue = new CellValue((Int32.Parse(c.CellValue.InnerText) + 1).ToString());
                    }
                    else if (c.CellFormula != null &&
                                c.CellValue != null)
                    {
                        /*
                         * Pour forcer la mise à jour des cellules ayant une formule, il faut en supprimer la valeur
                         * 
                         * /!\ La mise à jour ne se fera qu'à l'ouverture du fichier avec Excel
                         *  => Si le programme continu et qu'il
                         *      accède à cette même variable il échoura (c.CellValue sera null après cette instruction)
                         *      et ce même si il ferme puis réouvre spreadsheetDocument.
                         */
                        c.CellValue.Remove();
                    }
                }
                worksheetPart.Worksheet.Save();
                spreadsheetDocument.Close();
            }
        }

        /// <summary>
        /// Essai de créer un nouveau fichier de type xlsx.
        /// </summary>
        /// 
        /// <param name="pathToNewFile">
        /// Le chemin (nom et extension compris) du fichier à créer
        /// </param>
        /// 
        /// <returns>
        /// true  si la création réussie
        /// false si une erreur survient
        /// </returns>
        private static bool CreateWorkbook(string pathToNewFile, string sheetName)
        {
            try
            {
                // Create a spreadsheet document by supplying the filepath.
                // By default, AutoSave = true, Editable = true, and Type = xlsx.
                SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.
                        Create(pathToNewFile, SpreadsheetDocumentType.Workbook);

                // Add a WorkbookPart to the document.
                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                // Add a WorksheetPart to the WorkbookPart.
                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                // Add Sheets to the Workbook.
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                                        AppendChild<Sheets>(new Sheets());

                // Append a new worksheet and associate it with the workbook.
                Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                                            SheetId = 1, Name = sheetName };
                sheets.Append(sheet);

                // Close the document.
                workbookpart.Workbook.Save();
                spreadsheetDocument.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Supprime la cellule à la ligne rowIndex et la colonne colName
        ///     dans le tableau sheetName du fichier pathToFile.
        /// </summary>
        /// 
        /// <param name="pathToFile">
        /// Le chemin (nom et extension compris) du fichier à lire et modifier
        /// </param>
        /// <param name="sheetName">
        /// Le nom du tableau dans lequel supprimer la cellule
        /// </param>
        /// <param name="colName">
        /// Le nom de la colonne dans laquelle se trouve la cellule
        /// </param>
        /// <param name="rowIndex">
        /// Le numéro de la ligne dans laquelle se trouve la cellule
        /// </param>
        /// 
        /// <returns>
        /// true  si la cellule est effacée
        /// false si l'opération échoue
        /// </returns>
        private static bool RemoveCell(string pathToFile, string sheetName, string colName, int rowIndex)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(pathToFile, true))
            {
                // Récupération du sheet de nom sheetName
                IEnumerable<Sheet> sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>().Where(x => x.Name == sheetName);
                if (sheets.Count() == 0)
                {   // Le tableau sheet de nom sheetName n'existe pas
                    return false;
                }
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(relationshipId);

                // Récupération de la cellule de coordonées (rowIndex, colName)
                Cell cell = GetSpreadsheetCell(worksheetPart.Worksheet, colName, rowIndex);
                if (cell == null)
                {   // La cellule n'existe pas
                    return false;
                }

                cell.Remove();
                worksheetPart.Worksheet.Save();
                return true;
            }
        }

        /// <summary>
        /// Recherche et retourne la cellule dans la colonne colName et la ligne rowIndex
        ///     dans le worksheet passé en premier paramètre
        /// </summary>
        /// 
        /// <param name="worksheet">
        /// Le worksheet dans lequel il faut rechercher la cellule
        /// </param>
        /// <param name="colName">
        /// Le nom de la colonne dans laquelle se trouve la cellule
        /// </param>
        /// <param name="rowIndex">
        /// Le numéro de la ligne dans laquelle se trouve la cellule
        /// </param>
        /// 
        /// <returns>
        /// L'objet Cell trouvé
        /// null si aucune cellule n'est trouvée
        /// </returns>
        private static Cell GetSpreadsheetCell(Worksheet worksheet, string colName, int rowIndex)
        {
            IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Elements<Row>().Where(x => x.RowIndex == rowIndex);
            if (rows.Count() == 0)
            {   // La ligne n'a aucune cellule enregistrée
                return null;
            }

            IEnumerable<Cell> cells = rows.First<Row>().Elements<Cell>()
                                            .Where(x => string.Compare(x.CellReference.Value, colName + rowIndex, true) == 0);
            if (cells.Count() == 0)
            {   // La cellule (colName, rowIndex) n'existe pas
                return null;
            }

            return cells.First();
        }

        /// <summary>
        /// Change la valeur d'une cellule par une string.
        /// La cellule concernée se trouve à la colonne colName et la ligne rowIndex du tableau
        ///     sheetName lui-même dans le fichier pathToFile
        /// </summary>
        /// 
        /// 
        /// <param name="pathToFile">
        /// Le chemin (nom et extension compris) du fichier
        /// </param>
        /// <param name="sheetName">
        /// Le nom du tableau
        /// </param>
        /// <param name="colName">
        /// Le nom de la colonne dans laquelle se trouve la cellule
        /// </param>
        /// <param name="rowIndex">
        /// Le numéro de la ligne dans laquelle se trouve la cellule
        /// </param>
        /// <param name="newString">
        /// La nouvelle string à affecter
        /// </param>
        /// 
        /// <returns>
        /// true  si le remplacement s'est effectué
        /// false si une erreur est survenue
        /// </returns>
        private static bool ChangeStringValueInCell(string pathToFile, string sheetName, string colName,
                                                    int rowIndex, string newString)
        {
            // Ouverture du document
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(pathToFile, true))
            {
                // Récupération du tableau nommé "Feuil1"
                Sheet sheet = doc.WorkbookPart.Workbook.Descendants<Sheet>()
                                .Where(x => x.Name == sheetName).FirstOrDefault<Sheet>();
                if (sheet == null)
                {
                    return false;
                }
                WorksheetPart worksheetPart = (WorksheetPart) doc.WorkbookPart.GetPartById(sheet.Id);
                if (worksheetPart == null)
                {
                    return false;
                }
                // Récupération de la liste des string liées
                SharedStringTable listeString = doc.WorkbookPart.SharedStringTablePart.SharedStringTable;
                // Récupération de la liste des cellules
                IEnumerable<Cell> listCell = worksheetPart.Worksheet.Descendants<Cell>().Where(x => x.CellReference != null);

                /* Récupération de la cellule */
                Cell cellToModify = GetSpreadsheetCell(worksheetPart.Worksheet, colName, rowIndex);
                if (cellToModify == null)
                {
                    return false;
                }

                int indexOfString = Int32.Parse(cellToModify.CellValue.InnerText);
                listeString.ElementAt(indexOfString).Remove();
                SharedStringItem newSharedStringItem = new SharedStringItem() { Text = new DocumentFormat.OpenXml.Spreadsheet.Text(newString) };
                listeString.InsertAt(newSharedStringItem, indexOfString);

                worksheetPart.Worksheet.Save();
                doc.Close();
                return true;
            }
        }
        
        /// <summary>
        /// Remplace String dans une cellule par un entier
        /// </summary>
        /// 
        /// <param name="pathToFile">
        /// Le chemin (nom et extension compris) du fichier
        /// </param>
        /// <param name="sheetName">
        /// Le nom du tableau
        /// </param>
        /// <param name="colName">
        /// Le nom de la colonne dans laquelle se trouve la cellule
        /// </param>
        /// <param name="rowIndex">
        /// Le numéro de la ligne dans laquelle se trouve la cellule
        /// </param>
        /// <param name="newVal">
        /// La nouvelle valeur a affecter
        /// </param>
        public static bool ReplaceAStringValueByAnInt(string pathToFile, string sheetName, string colName,
                                                            int rowIndex, int newVal)
        {
            // Ouverture du document
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(pathToFile, true))
            {
                // Récupération du tableau nommé "Feuil1"
                Sheet sheet = doc.WorkbookPart.Workbook.Descendants<Sheet>()
                                .Where(x => x.Name == sheetName).FirstOrDefault<Sheet>();
                if (sheet == null)
                {
                    return false;
                }
                WorksheetPart worksheetPart = (WorksheetPart)doc.WorkbookPart.GetPartById(sheet.Id);
                if (worksheetPart == null)
                {
                    return false;
                }
                // Récupération de la liste des string liées
                SharedStringTable listeString = doc.WorkbookPart.SharedStringTablePart.SharedStringTable;
                // Récupération de la liste des cellules
                IEnumerable<Cell> listCell = worksheetPart.Worksheet.Descendants<Cell>().Where(x => x.CellReference != null);

                /* Récupération de la cellule */
                Cell cellToModify = GetSpreadsheetCell(worksheetPart.Worksheet, colName, rowIndex);
                if (cellToModify == null)
                {
                    return false;
                }

                int indexOfString = Int32.Parse(cellToModify.CellValue.InnerText);
                listeString.ElementAt(indexOfString).Remove();
                cellToModify.DataType = null;
                cellToModify.CellValue = new CellValue() { Text = newVal.ToString() };

                worksheetPart.Worksheet.Save();
                doc.Close();
                return true;
            }
        }

        /* ----- Fonction de test ----- */

        #region TestToPlaceAStringInCellValue
        /// <summary>
        /// Résultat du test
        /// Tentative de remplacement d'une CellValue par une autre.
        /// Comme attendu cela fonctionne sur le moment mais peut poser des problèmes par la suite.
        /// En effet si la cellule que l'on modifie référencait une string alors lors des lectures
        /// qui peuvent suivre (avec les algorithmes ci-dessus en tout cas) provoqueront des erreurs.
        /// L'explication est que les algorithmes de lectures détectent la présence de string via le
        /// champs DataType uniquement. Si c'est le cas alors il récupère l'index contenu dans CellValue
        /// pour le converti. Sauf que CellValue ne contient pas un Int32 mais une string.
        /// Cette erreur est évitable cependant cela ne serait pas une bonne chose étant donné que
        /// CellValue ne contient pas de string dans les cas nominaux.
        /// 
        /// La bonne pratique pour ce genre de fonction est de placer la string voulu dans la liste des
        /// string partagée (dans SharedString.xml) comme le fait la fonction ChangeStringByAnother et si
        /// besoin de procéder à un changement de DataType comme le fait la fonction ChangeIntByString.
        /// </summary>
        /// 
        /// <param name="pathToFile">
        /// Le chemin (nom et extension compris) du fichier à lire et modifier
        /// </param>
        private static void TestToPlaceAStringInValue(string pathToFile)
        {
            Console.WriteLine("Fichier : " + pathToFile);

            // Ouverture du document
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(pathToFile, true))
            {
                // Parcours de la hiérarchie du document jusqu'à récupération du tableau
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                // Récupération du tableau nommé "Feuil1"
                Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().Where(x => x.Name == "Feuil1").FirstOrDefault<Sheet>();
                WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                // Récupération de la liste des string liées
                SharedStringTable listeStringElement = workbookPart.SharedStringTablePart.SharedStringTable;
                // Récupération de la liste des cellules
                IEnumerable<Cell> listCell = worksheetPart.Worksheet.Descendants<Cell>().Where(x => x.CellReference != null);

                int i = 0;
                foreach (Cell c in listCell)
                {
                    if (c.CellFormula == null)
                    { // Pour ne pas toucher aux formules
                        c.CellValue = new CellValue("Test " + i);
                    }
                }

                worksheetPart.Worksheet.Save();
                spreadsheetDocument.Close();
            }
        }
        #endregion

        
    }
}
