using System;
using ChemSharp.Molecules;
using UnityEngine;
using TMPro; // Необходимо за TextMeshPro

public class MoleculeTest : MonoBehaviour
{
    public string path = @"Assets/graphene.cif"; // Път към файла с молекулярната структура
    public string modelFolder = @"ANSModels/"; // Директорият с моделите
    const float modelScaleFactor = 10f; // Скалиране на моделите
    const float scaleFactor = 200f; // Скалиране на позицията (конветиране на Ангстрьом в метри)
    public GameObject textPrefab; // Префаб за TextMeshPro 

    void Start()
    {
        // Зареждане на молекулата от файл
        var mol = Molecule.FromFile(path);

        // Изчисляване на центъра
        var moleculeCenter = Vector3.zero;
        foreach (var atom in mol.Atoms)
        {
            moleculeCenter += new Vector3(atom.Location.X, atom.Location.Y, atom.Location.Z);
        }
        moleculeCenter /= mol.Atoms.Count;

        // Зареждане на моделите на атомите
        var atomModels = new System.Collections.Generic.Dictionary<string, GameObject>
        {
            //{ "H", GameObject.CreatePrimitive(PrimitiveType.Sphere) },
            { "H", Resources.Load<GameObject>($"{modelFolder}Hydorgen Variant") },
            //{ "O", GameObject.CreatePrimitive(PrimitiveType.Sphere) },
            { "O", Resources.Load<GameObject>($"{modelFolder}Oxygen Variant") },
            { "Cu", Resources.Load<GameObject>($"{modelFolder}Cu Variant") },
            //{ "Cu", GameObject.CreatePrimitive(PrimitiveType.Sphere) },
            { "C", Resources.Load<GameObject>($"{modelFolder}Carbon Variant") },
            //{ "C", GameObject.CreatePrimitive(PrimitiveType.Sphere) },
            { "B", Resources.Load<GameObject>($"{modelFolder}B Variant") },
            { "Be", Resources.Load<GameObject>($"{modelFolder}Be Variant") },
            { "Br", Resources.Load<GameObject>($"{modelFolder}Br Variant") },
            { "Ca", Resources.Load<GameObject>($"{modelFolder}Ca Variant") },
            { "Cl", Resources.Load<GameObject>($"{modelFolder}Cl Variant") },
            { "Co", Resources.Load<GameObject>($"{modelFolder}Co Variant") },
            { "D", Resources.Load<GameObject>($"{modelFolder}D Variant") },
            { "F", Resources.Load<GameObject>($"{modelFolder}F Variant") },
            { "Fe", Resources.Load<GameObject>($"{modelFolder}Fe Variant") },
            { "Ga", Resources.Load<GameObject>($"{modelFolder}Ga Variant") },
            { "Ge", Resources.Load<GameObject>($"{modelFolder}Ge Variant") },
            { "H_2", Resources.Load<GameObject>($"{modelFolder}H_2 Variant") },
            { "He", Resources.Load<GameObject>($"{modelFolder}He Variant") },
            { "K", Resources.Load<GameObject>($"{modelFolder}K Variant") },
            { "Kr", Resources.Load<GameObject>($"{modelFolder}Kr Variant") },
            { "Li", Resources.Load<GameObject>($"{modelFolder}Li Variant") },
            { "Li2", Resources.Load<GameObject>($"{modelFolder}Li2 Variant") },
            { "Mg", Resources.Load<GameObject>($"{modelFolder}Mg Variant") },
            { "Mn", Resources.Load<GameObject>($"{modelFolder}Mn Variant") },
            { "Na", Resources.Load<GameObject>($"{modelFolder}Na Variant") },
            { "Ne", Resources.Load<GameObject>($"{modelFolder}Ne Variant") },
            { "Ni", Resources.Load<GameObject>($"{modelFolder}Ni Variant") },
            { "P", Resources.Load<GameObject>($"{modelFolder}P Variant") },
            { "S", Resources.Load<GameObject>($"{modelFolder}S Variant") },
            { "Sc", Resources.Load<GameObject>($"{modelFolder}Sc Variant") },
            { "Se", Resources.Load<GameObject>($"{modelFolder}Se Variant") },
            { "Si", Resources.Load<GameObject>($"{modelFolder}Si Variant") },
            { "T", Resources.Load<GameObject>($"{modelFolder}T Variant") }
            //{ "N", GameObject.CreatePrimitive(PrimitiveType.Sphere) }
            //{ "N", Resources.Load<GameObject>($"{modelFolder}Nitrogen") },
            //{ "Cu", Resources.Load<GameObject>($"{modelFolder}Copper") }
            // Добавяне и на останалите атоми
        };

        // Рендване на атомите
        //foreach (var atom in mol.Atoms)
        //{
        //    if (atomModels.TryGetValue(atom.Symbol, out var atomModel))
        //    {
        //        //Инстанцииране на модел
        //        var obj = Instantiate(atomModel,
        //            new Vector3(atom.Location.X, atom.Location.Y, atom.Location.Z),
        //            Quaternion.identity);

        //        // Скалиране, базирано на ковалентния радиус
        //        obj.transform.localScale = Vector3.one * (atom.CovalentRadius / 100f);

        //        // Избор на цвят (опция, ако моделът няма подходящ материал)
        //        var renderer = obj.GetComponent<Renderer>();
        //        if (renderer != null)
        //        {
        //            var mat = new Material(Shader.Find("Standard"));
        //            ColorUtility.TryParseHtmlString(atom.Color, out var col);
        //            mat.color = col;
        //            renderer.material = mat;
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogWarning($"No FBX model found for atom type: {atom.Symbol}");
        //    }
        //}

        //foreach (var atom in mol.Atoms)
        //{
        //    if (atomModels.TryGetValue(atom.Symbol, out var atomModel))
        //    {
        //        Debug.Log($"Instantiating model for {atom.Symbol} at position {atom.Location}");
        //        // Instantiate the FBX model
        //        //obj.transform.position = new Vector3(atom.Location.X, atom.Location.Y, atom.Location.Z);
        //        //var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //        var obj = Instantiate(atomModel,
        //            new Vector3(atom.Location.X, atom.Location.Y, atom.Location.Z),
        //            Quaternion.identity);

        //        //reposition the atom
        //        //transform.position = new Vector3(atom.Location.X, atom.Location.Y, atom.Location.Z);

        //        // Scale based on covalent radius
        //        //obj.transform.localScale = Vector3.one * (atom.CovalentRadius / 100f);
        //        obj.transform.localScale = Vector3.one * (atom.CovalentRadius / 100f) * scaleFactor * modelScaleFactor;

        //        // Apply color (optional, if FBX doesn't already have appropriate material)
        //        var renderer = obj.GetComponent<Renderer>();
        //        if (renderer != null)
        //        {
        //            var mat = new Material(Shader.Find("Standard"));
        //            ColorUtility.TryParseHtmlString(atom.Color, out var col);
        //            mat.color = col;
        //            renderer.material = mat;
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogWarning($"No FBX model found for atom type: {atom.Symbol}");
        //    }
        //}



        foreach (var atom in mol.Atoms)
        {
            if (atomModels.TryGetValue(atom.Symbol, out var atomModel))
            {
                //// Изчисляване на ротациите
                var axis = Vector3.Cross(Vector3.up, Vector3.up * 0.2f);
                var angle = Mathf.Acos(Vector3.Dot(Vector3.up, Vector3.up * 0.2f));
                var rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, axis);

                // Настройване на позицията и центъра на молекулите
                var position = new Vector3(atom.Location.X, atom.Location.Y, atom.Location.Z) - moleculeCenter;
                //var position = new Vector3(
                //        atom.Location.X * scaleFactor,
                //        atom.Location.Y * scaleFactor,
                //        atom.Location.Z * scaleFactor
                //    ) - moleculeCenter * scaleFactor;

                Debug.Log($"Placing {atom.Symbol} at {position}");

                // Инстанцииране на модела и позициониране
                //var sph = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                var obj = Instantiate(atomModel, position + Vector3.up * 0.5f, Quaternion.identity);
                obj.transform.rotation = rotation;
                //obj.transform.localScale = Vector3.one * modelScaleFactor; // Скалиране на атомния модел

                // Създаване на текстов обект (TextMeshPro)
                if (textPrefab != null)
                {
                    var textObj = Instantiate(textPrefab, position + Vector3.up * 0.27f, Quaternion.identity);
                    var textMesh = textObj.GetComponent<TextMeshPro>();
                    if (textMesh != null)
                    {
                        textMesh.text = atom.Symbol; // Показване на символа
                        textMesh.fontSize = 3f; // Настройка на размера на текста
                        textMesh.alignment = TextAlignmentOptions.Center;
                    }
                }
            }
            else
            {
                Debug.LogWarning($"No FBX model found for atom type: {atom.Symbol}");
            }
        }



        // Рендване на връзките(засега ползваме цилиндри)
        foreach (var bond in mol.Bonds)
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            //// Изчисляване на позициите
            //var start = new Vector3(bond.Atom1.Location.X, bond.Atom1.Location.Y, bond.Atom1.Location.Z);
            //var end = new Vector3(bond.Atom2.Location.X, bond.Atom2.Location.Y, bond.Atom2.Location.Z);
            //var loc = Vector3.Lerp(start, end, 0.5f); // Midpoint for bond placement
            //var lineVector = end - start;

            //// Изчисляване на ротациите
            //var axis = Vector3.Cross(Vector3.up, lineVector.normalized);
            //var angle = Mathf.Acos(Vector3.Dot(Vector3.up, lineVector.normalized));
            //var rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, axis);

            //// Прилагане на трансформациите
            //obj.transform.localScale = new Vector3(0.05f, lineVector.magnitude / 2f, 0.05f);
            //obj.transform.position = loc;
            //obj.transform.rotation = rotation;


            //var obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            // Изчисляване на позициите
            var start = new Vector3(bond.Atom1.Location.X, bond.Atom1.Location.Y, bond.Atom1.Location.Z);
            var end = new Vector3(bond.Atom2.Location.X, bond.Atom2.Location.Y, bond.Atom2.Location.Z);
            var loc = Vector3.Lerp(start, end, 0.5f);
            var lineVector = end - start;

            // Изчисляване на ротациите
            var angle = Mathf.Acos(Vector3.Dot(Vector3.up, lineVector.normalized));
            var axis = Vector3.Cross(Vector3.up, lineVector.normalized);
            var rad = Mathf.Acos(Vector3.Dot(Vector3.up, lineVector.normalized));
            var matrix = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, axis);

            //// Прилагане на трансформациите
            obj.transform.localScale = new Vector3(0.05f, lineVector.magnitude / 2f, 0.05f);
            obj.transform.position = loc;
            obj.transform.rotation = matrix;


            //var objatom = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //obj.transform.position = new Vector3(bond.Atom1.Location.X, bond.Atom1.Location.Y, bond.Atom1.Location.Z);
            //obj.transform.localScale =
            //    new Vector3(atom.CovalentRadius, atom.CovalentRadius, atom.CovalentRadius) / 100f;
            //var mat = new Material(Shader.Find("Standard"));
            //ColorUtility.TryParseHtmlString(atom.Color, out var col);
            //mat.color = col;
            //obj.GetComponent<Renderer>().material = mat;

        }
    }

}
