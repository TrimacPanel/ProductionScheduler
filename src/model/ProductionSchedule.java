package model;

import javafx.collections.FXCollections;
import javafx.collections.ObservableList;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.sql.*;
import java.util.Properties;

public class ProductionSchedule {
    private static ObservableList<OpenWorkOrder> prodSchWorkOrders = FXCollections.observableArrayList();

    ProductionSchedule(){
        ObservableList<WorkOrder> macolaData = retrieveMacolaData(); //retrieve macola data
        ObservableList<OpenWorkOrder> productionScheduleData = retrieveProductionScheduleData(); //retrieve production schedule data

        // compare the two containers for, existence, status completion, and then deletion
        // Put processed data into prodSchWorkOrders
        prodSchWorkOrders = compareMacolaWithProductionScheduleData(macolaData, productionScheduleData);
    }

    /*
    refreshes the production schedule data by checking the Macola data
     */
    public static void refreshProductionScheduleData() {
        //Saves the production schedule OpenWorkOrders first, before doing a comparison and refreshing the data.
        saveProductionScheduleData();

        ObservableList<WorkOrder> macolaData = retrieveMacolaData(); //retrieve macola data
        ObservableList<OpenWorkOrder> productionScheduleData = retrieveProductionScheduleData(); //retrieve production schedule data

        // compare the two containers for, existence, status completion, and then deletion
        // Put processed data into prodSchWorkOrders
        prodSchWorkOrders = compareMacolaWithProductionScheduleData(macolaData, productionScheduleData);
    }

    /*
    compareMacolaWithProductionScheduleData is the logic for checking existence, status, and possible deleting data
    from the Production Schedule Dataset
     */
    private static ObservableList<OpenWorkOrder> compareMacolaWithProductionScheduleData(ObservableList<WorkOrder> macolaData, ObservableList<OpenWorkOrder> productionScheduleData){
        //TODO: compare Production Schedule Data with Macola Data


        return null; //Return Open Work Objects that contain all the data from both
    }

    /*
    retrieveProductionScheduleData method accesses the database that the Production Schedule uses to save it's
    custom fields and then loads that data in to OpenWorkOrder objects.
     */
    private static ObservableList<OpenWorkOrder> retrieveProductionScheduleData(){
                /*
        config.properties looks like this:
        db.url=Your url
        db.dbName=Your database name
        db.user=Your username
        db.password=Your password
         */

        String url = "";
        String dbName = "";
        String user = "";
        String password = "";

        Properties prop = new Properties();
        try (InputStream input = new FileInputStream("src/resources/productionscheduleconfig.properties")){
            prop.load(input);
            url = prop.getProperty("db.url");
            dbName = prop.getProperty("db.dbName");
            user = prop.getProperty("db.user");
            password = prop.getProperty("db.password");
        } catch (IOException ex) {
            ex.printStackTrace();
        }

        String connectionUrl = "jdbc:sqlserver://" + url + ";databaseName=" + dbName + ";user=" + user + ";password=" + password;
        ResultSet rs = null;
        ObservableList<OpenWorkOrder> productionScheduleData = FXCollections.observableArrayList();

        try (Connection con = DriverManager.getConnection(connectionUrl); Statement stmt = con.createStatement();) {
            String SQL = "SELECT * FROM [ProductionSchedule].[dbo].[Open_Work_Orders];";
            rs = stmt.executeQuery(SQL);

            while(rs.next()){
                //Load Macola Data into Work Order object for processing
                productionScheduleData.add(new OpenWorkOrder(rs.getDate("scheduled_date"),
                        rs.getString("production_line"), rs.getString("work_shift"),
                        rs.getInt("priority"), null, rs.getInt("work_order_num"),
                        null, 0, null, null,
                        null, null, 0, null));
            }
        }
        // Handle any errors that may have occurred.
        catch (SQLException e) {
            e.printStackTrace();
        }

        return productionScheduleData;
    }

    /*
    retrieveMacolaData method accesses the Macola server and loads the data into WorkOrder objects.
     */
    private static ObservableList<WorkOrder> retrieveMacolaData(){
        /*
        config.properties looks like this:
        db.url=Your url
        db.dbName=Your database name
        db.user=Your username
        db.password=Your password
         */

        String url = "";
        String dbName = "";
        String user = "";
        String password = "";

        Properties prop = new Properties();
        try (InputStream input = new FileInputStream("src/resources/macolaconfig.properties")){
            prop.load(input);
            url = prop.getProperty("db.url");
            dbName = prop.getProperty("db.dbName");
            user = prop.getProperty("db.user");
            password = prop.getProperty("db.password");
        } catch (IOException ex) {
            ex.printStackTrace();
        }

        String connectionUrl = "jdbc:sqlserver://" + url + ";databaseName=" + dbName + ";user=" + user + ";password=" + password;
        ResultSet rs = null;
        ObservableList<WorkOrder> macolaData = FXCollections.observableArrayList();

        try (Connection con = DriverManager.getConnection(connectionUrl); Statement stmt = con.createStatement();) {
            String SQL = "SELECT * FROM [DATA_01].[dbo].[vPROD_OpenOrd_PPI];";
            rs = stmt.executeQuery(SQL);

            while(rs.next()){
                //TODO: Need to trim some of the strings
                //Load Macola Data into Work Order object for processing
                macolaData.add(new WorkOrder(rs.getDate("due_dt"), rs.getInt("ord_no"),
                        rs.getString("cus_name").trim(), rs.getInt("ord_qty"),
                        rs.getString("item_no").trim(), rs.getString("item_desc_1").trim(),
                        rs.getString("sub_item").trim(), rs.getString("ovly_item").trim(),
                        rs.getInt("unit_size"), rs.getString("user_def_fld_2").trim()));
            }
        }
        // Handle any errors that may have occurred.
        catch (SQLException e) {
            e.printStackTrace();
        }

        return macolaData;
    }

    /*
    Saves the OpenWorkOrder objects in memory to the production schedule database
     */
    public static void saveProductionScheduleData(){
        //TODO Save the OpenWorkOrder objects in memory to the production schedule database
    }

    public static ObservableList<OpenWorkOrder> getProdSchWorkOrders() {
        return prodSchWorkOrders;
    }

    public static void setProdSchWorkOrders(ObservableList<OpenWorkOrder> argProdSchWorkOrders) {
        prodSchWorkOrders = argProdSchWorkOrders;
    }
}
