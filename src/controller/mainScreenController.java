package controller;

import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.control.TableColumn;
import javafx.scene.control.TableView;
import javafx.scene.input.KeyEvent;
import model.OpenWorkOrder;
import model.ProductionSchedule;

import java.util.Date;

public class mainScreenController {

    @FXML
    private TableView<OpenWorkOrder> prodSchTableView;

    @FXML
    private TableColumn<OpenWorkOrder, String> prodLineCol;

    @FXML
    private TableColumn<OpenWorkOrder, Date> schDateCol;

    @FXML
    private TableColumn<OpenWorkOrder, Integer> seqCol;

    @FXML
    private TableColumn<OpenWorkOrder, Date> dueDateCol;

    @FXML
    private TableColumn<OpenWorkOrder, Integer> workOrderNumCol;

    @FXML
    private TableColumn<OpenWorkOrder, String> macRefCol;

    @FXML
    private TableColumn<OpenWorkOrder, Integer> qtyCol;

    @FXML
    private TableColumn<OpenWorkOrder, String> finishedSkuCol;

    @FXML
    private TableColumn<OpenWorkOrder, String> itemDescCol;

    @FXML
    private TableColumn<OpenWorkOrder, String> rawSkuCol;

    @FXML
    private TableColumn<OpenWorkOrder, String> filmSkuCol;

    @FXML
    private TableColumn<OpenWorkOrder, Integer> faceCol;

    @FXML
    private TableColumn<OpenWorkOrder, Integer> numOfUnitsCol;

    @FXML
    private TableColumn<OpenWorkOrder, Integer> pieceCountCol;

    @FXML
    void onActionFileMenuClose(ActionEvent event) {
        System.exit(0);
    }

    @FXML
    void onActionRefreshButton(ActionEvent event) {
        //TODO Refresh Button
     }

    @FXML
    void onKeyPressRefreshButton(KeyEvent event) {
        //TODO hotkeyToRefreshButton
    }

}
