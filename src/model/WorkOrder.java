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
    private int unitSize;
    private String salesOrder;

    WorkOrder(Date dueDate, int workOrderNumber, String macolaReference, int quantity, String finishedSKU, String itemDescription,
              String substrateSKU, String filmSKU, int unitSize, String salesOrder) {
        this.dueDate = dueDate;
        this.ID = workOrderNumber;
        this.macolaRef = macolaReference;
        this.qty = quantity;
        this.finishedSKU = finishedSKU;
        this.itemDesc = itemDescription;
        this.subSKU = substrateSKU;
        this.filmSKU = filmSKU;
        this.unitSize = unitSize;
        this.salesOrder = salesOrder;
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

    public int getUnitSize() {
        return unitSize;
    }

    public void setUnitSize(int unitSize) {
        this.unitSize = unitSize;
    }

    public String getSalesOrder() {
        return salesOrder;
    }

    public void setSalesOrder(String salesOrder) {
        this.salesOrder = salesOrder;
    }
}
