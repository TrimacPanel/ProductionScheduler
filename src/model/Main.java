package model;

import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.stage.Stage;

public class Main extends Application {

    @Override
    public void start(Stage primaryStage) throws Exception{
        Parent root = FXMLLoader.load(getClass().getResource("/view/mainscreen.fxml"));
        primaryStage.setTitle("Production Scheduler");
        primaryStage.setScene(new Scene(root, 1000, 700));
        primaryStage.show();
    }


    public static void main(String[] args) {
         //TODO Load Open Work Orders
        //TODO Do a comparison between Macola and current Data, and add new work orders
        ProductionSchedule productionSchedule = new ProductionSchedule();
        launch(args);
    }
}
