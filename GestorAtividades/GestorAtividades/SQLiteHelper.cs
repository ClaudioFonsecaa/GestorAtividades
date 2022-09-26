using Android.App;
using Android.OS;
using GestorAtividades.Models;
using Java.Nio.Channels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Android.Content.ClipData;
using static Android.Resource;
using static Java.Text.Normalizer;

namespace GestorAtividades
{
    public class SQLiteHelper
    {

        //Ficheiro que contem as CRUD OPERATION das tabelas que guardam as atividades doing, done e todo activities

        private readonly SQLiteAsyncConnection db;

        public SQLiteHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            var database = new SQLiteConnection(dbPath);
            db.CreateTableAsync<doingTable>();
            db.CreateTableAsync<doneTable>();
            db.CreateTableAsync<todoTable>();
        }



        /* INSERIR AS ATIVIDADES -------------------------------------------------- */
        public Task<int> InsertActivity(object sender)
        {
            return db.InsertAsync(sender);
        }

        /* FIM INSERIR AS ATIVIDADES -------------------------------------------------- */


        /* LER AS ATIVIDADES -------------------------------------------------- */

        public Task<List<doingTable>> ReadIDDoingActivity(string categoria, int id)
        {
            return db.Table<doingTable>().Where(p => p.Id.Equals(id)).ToListAsync();
        }
        public Task<List<doneTable>> ReadIDDoneActivity(string categoria, int id)
        {
            return db.Table<doneTable>().Where(p => p.Id.Equals(id)).ToListAsync();
        }
        public Task<List<todoTable>> ReadIDTodoActivity(string categoria, int id)
        {
            return db.Table<todoTable>().Where(p => p.Id.Equals(id)).ToListAsync();

        }

   

        public Task<List<doingTable>> ReadDoingActivity()
        {
            return db.Table<doingTable>().ToListAsync();
        }
        public Task<List<doneTable>> ReadDoneActivity()
        {
            return db.Table<doneTable>().ToListAsync();
        }
        public Task<List<todoTable>> ReadTodoActivity()
        {
            return db.Table<todoTable>().ToListAsync();
        }

        /* FIM LER AS ATIVIDADES ----------------------------------------------------- */




        /* ATUALIZAR AS ATIVIDADES -------------------------------------------------- */
        public Task<int> UpdateActivity(object sender)
        {
            return db.UpdateAsync(sender);
        }


        public async Task<List<doingTable>> UpdateDoingAlarmeByID(int NotificationID)
        {
           
            return await db.QueryAsync<doingTable>("Update doingTable set Alarme=false where NotificationID=?", NotificationID);

        }

        public async Task<List<doneTable>> UpdateDoneAlarmeByID(int NotificationID)
        {

            return await db.QueryAsync<doneTable>("Update doneTable set Alarme=false where NotificationID=?", NotificationID);

        }

        public async Task<List<todoTable>> UpdateTodoAlarmeByID(int NotificationID)
        {

            return await db.QueryAsync<todoTable>("Update todoTable set Alarme=false where NotificationID=?", NotificationID);

        }


        /* FIM ATUALIZAR AS ATIVIDADES -------------------------------------------------- */



        /* APAGAR AS ATIVIDADES -------------------------------------------------- */
        public Task<int> DeleteActivity(object sender)
        {
            return db.DeleteAsync(sender);
        }
        /* FIM APAGAR AS ATIVIDADES -------------------------------------------------- */



        /* PROCURAR AS ATIVIDADES -------------------------------------------------- */
        public Task<List<doingTable>>SearchDoingActivity(string search)
        {
            return db.Table<doingTable>().Where(p => p.Name.StartsWith(search)).ToListAsync();
        }

        public Task<List<doneTable>> SearchDoneActivity(string search)
        {
            return db.Table<doneTable>().Where(p => p.Name.StartsWith(search)).ToListAsync();
        }

        public Task<List<todoTable>> SearchTodoActivity(string search)
        {
            return db.Table<todoTable>().Where(p => p.Name.StartsWith(search)).ToListAsync();
        }

        /* FIM DE PROCURAR AS ATIVIDADES -------------------------------------------------- */


       


    }
}
