﻿using atelier2.model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace atelier2.dal
{
    /// <summary>
    /// Classe permettant de gérer les demandes concernant les absences
    /// </summary>
    public class AbsenceAccess
    {
        /// <summary>
        /// Instance unique de l'accès aux données
        /// </summary>
        private readonly Access access = null;

        /// <summary>
        /// Constructeur pour créer l'accès aux données
        /// </summary>
        public AbsenceAccess()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// Récupère et retourne les absences d'un personnel
        /// </summary>
        /// <param name="personnel">le personnel concerné par les absences</param>
        /// <returns>liste d'absences</returns>
        public List<Absence> GetLesAbsences(Personnel personnel)
        {
            List<Absence> lesAbsences = new List<Absence>();
            if (access.Manager != null)
            {
                string req = "select a.idpersonnel as idpersonnel, a.datedebut as datedebut, a.datefin as datefin, a.idmotif as idmotif, m.libelle as libelle ";
                req += "from absence a join motif m on (a.idmotif = m.idmotif) ";
                req += "where idpersonnel = @idpersonnel ";
                req += "order by datedebut desc ";
                try
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("@idpersonnel",personnel.Idpersonnel);
                    List<Object[]> records = access.Manager.ReqSelect(req, parameters);
                    if (records != null)
                    {
                        foreach (Object[] record in records)
                        {
                            Motif motif = new Motif((int)record[3], (string)record[4]);
                            Absence absence = new Absence((int)record[0], (DateTime)record[1], (DateTime)record[2], motif);
                            lesAbsences.Add(absence);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + "!", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
            }
            return lesAbsences;
        }

        /// <summary>
        /// Demande d'ajout d'une absence
        /// </summary>
        /// <param name="absence">objet absence à ajouter</param>
        public void AddAbsence(Absence absence)
        {
            if (access.Manager != null)
            {

                string req = "insert into absence(idpersonnel, datedebut, datefin, idmotif) ";
                req += "values (@idpersonnel, @datedebut, @datefin, @idmotif);";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@idpersonnel", absence.Idpersonnel);
                parameters.Add("@datedebut", absence.Datedebut.ToString("yyyy-MM-dd HH:mm:ss"));
                parameters.Add("@datefin", absence.Datefin.ToString("yyyy-MM-dd HH:mm:ss"));
                parameters.Add("@idmotif", absence.Motif.Idmotif);
                try
                {
                    access.Manager.ReqUpdate(req, parameters);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + "!", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// Demande de modification d'uen absence
        /// </summary>
        /// <param name="absence">objet absence à modifier</param>
        public void UpdateAbsence(Absence absence)
        {
            if (access.Manager != null)
            {
                string req = "update absence set idpersonnel = @idpersonnel, datedebut = @datedebut, datefin = @datefin, idmotif = @idmotif ";
                req += "where idpersonnel = @idpersonnel and datedebut = @datedebut ;";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@idpersonnel", absence.Idpersonnel);
                parameters.Add("@datedebut", absence.Datedebut.ToString("yyyy-MM-dd HH:mm:ss"));
                parameters.Add("@datefin", absence.Datefin.ToString("yyyy-MM-dd HH:mm:ss"));
                parameters.Add("@idmotif", absence.Motif.Idmotif);
                try
                {
                    access.Manager.ReqUpdate(req, parameters);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + "!", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// Demande de suppression d'un absence
        /// </summary>
        /// <param name="absence">objet absence à supprimer</param>
        public void DelAbsence(Absence absence)
        {
            if (access.Manager != null)
            {
                string req = "delete from absence where datedebut = @datedebut and idpersonnel = @idpersonnel;";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@idpersonnel", absence.Idpersonnel);
                parameters.Add("@datedebut", absence.Datedebut);
                try
                {
                    access.Manager.ReqUpdate(req, parameters);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + "!", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// Demande de suppression des absences d'une personnel
        /// </summary>
        /// <param name="personnel">le personnel dont on effasse les absences</param>
        public void DelLesAbsences(Personnel personnel)
        {
            if (access.Manager != null)
            {
                string req = "delete from absence where idpersonnel = @idpersonnel ;";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@idpersonnel", personnel.Idpersonnel);
                try
                {
                    access.Manager.ReqUpdate(req, parameters);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + "!", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
            }
        }   
    }
}


