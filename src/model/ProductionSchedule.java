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
        /*
        TODO Secure userName and Password for public use. Not sure how to do that.
        Check out java properties
        https://www.mkyong.com/java/java-properties-file-examples/
        */

        String url = "";
        String dbName = "";
        String user = "";
        String password = "";

        Properties prop = new Properties();
        try (InputStream input = new FileInputStream("src/resources/config.properties")){
            prop.load(input);
            url = prop.getProperty("db.url");
            dbName = prop.getProperty("db.dbName");
            user = prop.getProperty("db.user");
            password = prop.getProperty("db.password");
        } catch (IOException ex) {
            ex.printStackTrace();
        }


        String connectionUrl = "jdbc:sqlserver://" + url + ";databaseName=" + dbName + ";user=" + user + ";password=" + password;

        try (Connection con = DriverManager.getConnection(connectionUrl); Statement stmt = con.createStatement();) {
            String SQL = "SELECT * FROM [DATA_01].[dbo].[vPROD_OpenOrd_PPI];";
            ResultSet rs = stmt.executeQuery(SQL);
            while (rs.next()) {
                System.out.println(rs.getString("ord_no") + " : " + rs.getString("item_no"));
            }
        }
        // Handle any errors that may have occurred.
        catch (SQLException e) {
            e.printStackTrace();
        }
    }

    public static ObservableList<OpenWorkOrder> getProdSchWorkOrders() {
        return prodSchWorkOrders;
    }

    public static void setProdSchWorkOrders(ObservableList<OpenWorkOrder> argProdSchWorkOrders) {
        prodSchWorkOrders = argProdSchWorkOrders;
    }
}
