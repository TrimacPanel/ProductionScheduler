package model;

import java.util.Date;

public class WorkOrder {
    private Date dueDate;
    private int ID;
    private String macolaRef;
    private int qty;
    private String finishedSKU;
    private String itemDesc;
    private String subSKU;
    private String filmSKU;
    private int faces;
    private int numOfUnits;
    private int pieceCount;

    WorkOrder(Date dueDate, int workOrderNumber, String macolaReference, int quantity, String finishedSKU, String itemDescription,
              String substrateSKU, String filmSKU, int numberOfFaces, int numberOfUnits, int pieceCount){
        this.dueDate = dueDate;
        this.ID = workOrderNumber;
        this.macolaRef = macolaReference;
        this.qty = quantity;
        this.finishedSKU = finishedSKU;
        this.itemDesc = itemDescription;
        this.subSKU = substrateSKU;
        this.filmSKU = filmSKU;
        this.faces = numberOfFaces;
        this.numOfUnits = numberOfUnits;
        this.pieceCount = pieceCount;
    }

    public Date getDueDate() {
        return dueDate;
    }

    public void setDueDate(Date dueDate) {
        this.dueDate = dueDate;
    }

    public int getID() {
        return ID;
    }

    public void setID(int ID) {
        this.ID = ID;
    }

    public String getMacolaRef() {
        return macolaRef;
    }

    public void setMacolaRef(String macolaRef) {
        this.macolaRef = macolaRef;
    }

    public int getQty() {
        return qty;
    }

    public void setQty(int qty) {
        this.qty = qty;
    }

    public String getFinishedSKU() {
        return finishedSKU;
    }

    public void setFinishedSKU(String finishedSKU) {
        this.finishedSKU = finishedSKU;
    }

    public String getItemDesc() {
        return itemDesc;
    }

    public void setItemDesc(String itemDesc) {
        this.itemDesc = itemDesc;
    }

    public String getSubSKU() {
        return subSKU;
    }

    public void setSubSKU(String subSKU) {
        this.subSKU = subSKU;
    }

    public String getFilmSKU() {
        return filmSKU;
    }

    public void setFilmSKU(String filmSKU) {
        this.filmSKU = filmSKU;
    }

    public int getFaces() {
        return faces;
    }

    public void setFaces(int faces) {
        this.faces = faces;
    }

    public int getNumOfUnits() {
        return numOfUnits;
    }

    public void setNumOfUnits(int numOfUnits) {
        this.numOfUnits = numOfUnits;
    }

    public int getPieceCount() {
        return pieceCount;
    }

    public void setPieceCount(int pieceCount) {
        this.pieceCount = pieceCount;
    }
}
