var GridRow = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 
		};  
	},
	delCheck:function(i,chd){
		this.props.delCheck(i,chd);
	},
	modify:function(){
		this.props.updateType(this.props.primKey);
	},
	render:function(){
		return (

				<tr>
					<td className="text-center"><GridCheckDel iKey={this.props.ikey} chd={this.props.itemData.check_del} delCheck={this.delCheck} /></td>
					<td className="text-center"><GridButtonModify modify={this.modify}/></td>
					<td>{this.props.itemData.l1_name}</td>
					<td>{this.props.itemData.l2_name}</td>
					<td>{this.props.itemData.product_name}</td>
					<td>{this.props.itemData.sort}</td>
					<td>{this.props.itemData.i_Hide?<span className="label label-default">隱藏</span>:<span className="label label-primary">顯示</span>}</td>
				</tr>
			);
		}
});

//主表單
var GirdForm = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return {
			gridData:{rows:[],page:1},
			fieldData:{},
			searchData:{title:null},
			edit_type:0,
			checkAll:false,
			category_l1:[],
			category_l2:[]
		};  
	},
	getDefaultProps:function(){
		return{	
			fdName:'fieldData',
			gdName:'searchData',
			apiPathName:gb_approot+'api/Product',
			initPathName:gb_approot+'Active/ProductData/product_Init'
		};
	},	
	componentDidMount:function(){
		this.queryGridData(1);
		this.getAjaxInitData();//載入init資料
	},
	shouldComponentUpdate:function(nextProps,nextState){
		return true;
	},
	componentDidUpdate:function(prevProps, prevState){
        /*
            在元件更新之後執行。這個方法同樣不在初始化時執行，使用時機為當元件被更新之後需要執行一些操作。
        */
        //設定新增時的編輯器
        if(prevState.edit_type==0 && this.state.edit_type==1){
            CKEDITOR.replace( 'editor1', {});
        }
    },
	getAjaxInitData:function(){
		jqGet(this.props.initPathName)
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({category_l1:data.options_category});
			//載入下拉是選單內容
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	handleSubmit: function(e) {

		e.preventDefault();
		this.state.fieldData.product_content = CKEDITOR.instances.editor1.getData();//編輯器

		if(this.state.edit_type==1){
			jqPost(this.props.apiPathName,this.state.fieldData)
			.done(function(data, textStatus, jqXHRdata) {
				if(data.result){
					if(data.message!=null){
						tosMessage(null,'新增完成'+data.message,1);
					}else{
						tosMessage(null,'新增完成',1);
					}
					this.updateType(data.id);
				}else{
					tosMessage(null,data.message,3);
				}
			}.bind(this))
			.fail(function( jqXHR, textStatus, errorThrown ) {
				showAjaxError(errorThrown);
			});
		}		
		else if(this.state.edit_type==2){
			jqPut(this.props.apiPathName,this.state.fieldData)
			.done(function(data, textStatus, jqXHRdata) {
				if(data.result){
					if(data.message!=null){
						tosMessage(null,'修改完成'+data.message,1);
					}else{
						tosMessage(null,'修改完成',1);
					}
				}else{
					tosMessage(null,data.message,3);
				}
			}.bind(this))
			.fail(function( jqXHR, textStatus, errorThrown ) {
				showAjaxError(errorThrown);
			});
		};
		return;
	},
	deleteSubmit:function(e){

		if(!confirm('確定是否刪除?')){
			return;
		}

		var ids = [];
		for(var i in this.state.gridData.rows){
			if(this.state.gridData.rows[i].check_del){
				ids.push('ids='+this.state.gridData.rows[i].product_id);
			}
		}

		if(ids.length==0){
			tosMessage(null,'未選擇刪除項',2);
			return;
		}

		jqDelete(this.props.apiPathName + '?' + ids.join('&'),{})			
		.done(function(data, textStatus, jqXHRdata) {
			if(data.result){
				tosMessage(null,'刪除完成',1);
				this.queryGridData(0);
			}else{
				tosMessage(null,data.message,3);
			}
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	handleSearch:function(e){
		e.preventDefault();
		this.queryGridData(0);
		return;
	},
	delCheck:function(i,chd){

		var newState = this.state;
		this.state.gridData.rows[i].check_del = !chd;
		this.setState(newState);
	},
	checkAll:function(){

		var newState = this.state;
		newState.checkAll = !newState.checkAll;
		for (var prop in this.state.gridData.rows) {
			this.state.gridData.rows[prop].check_del=newState.checkAll;
		}
		this.setState(newState);
	},
	gridData:function(page){

		var parms = {
			page:0
		};

		if(page==0){
			parms.page=this.state.gridData.page;
		}else{
			parms.page=page;
		}

		$.extend(parms, this.state.searchData);

		return jqGet(this.props.apiPathName,parms);
	},
	queryGridData:function(page){
		this.gridData(page)
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({gridData:data});
		}.bind(this))
		.fail(function(jqXHR, textStatus, errorThrown) {
			showAjaxError(errorThrown);
		});
	},
	insertType:function(){
		var defaultL1=this.state.category_l1;
		var defaultL2=defaultL1[0].l2_list;
		this.setState({edit_type:1,fieldData:{l1_id:defaultL1[0].l1_id,l2_id:defaultL2[0].l2_id,product_type:1}});
	},
	updateType:function(id){
		jqGet(this.props.apiPathName,{id:id})
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({edit_type:2,fieldData:data.data});
			CKEDITOR.replace( 'editor1', {});
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	noneType:function(){
		this.gridData(0)
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({edit_type:0,gridData:data});
		}.bind(this))
		.fail(function(jqXHR, textStatus, errorThrown) {
			showAjaxError(errorThrown);
		});
	},
	changeFDValue:function(name,e){
		this.setInputValue(this.props.fdName,name,e);
	},
	changeGDValue:function(name,e){
		this.onSearchDataChange(name,e);
	},
	setFDValue:function(fieldName,value){
		//此function提供給次元件調用，所以要以屬性往下傳。
		var obj = this.state[this.props.fdName];
		obj[fieldName] = value;
		this.setState({fieldData:obj});
	},
	setInputValue:function(collentName,name,e){

		var obj = this.state[collentName];
		if(e.target.value=='true'){
			obj[name] = true;
		}else if(e.target.value=='false'){
			obj[name] = false;
		}else{
			obj[name] = e.target.value;
		}
		this.setState({fieldData:obj});
	},
	onL1Change: function (e) {
        this.listL2(e.target.value);
        var obj = this.state.searchData;
        obj['l1_id'] = e.target.value;
        this.setState({ searchData: obj });
    },
    onSearchDataChange: function (name,e) {
        var obj = this.state.searchData;
        obj[name] = e.target.value;
        this.setState({ searchData: obj });
    },
    listL2: function (value) {
		$("#search-l2 option:first").attr("selected", true);
		var searchData=this.state.searchData;
		searchData.l2_id=null;
		
    	var category_l1=this.state.category_l1;
        for (var i in category_l1) {
            var item = category_l1[i];
            if (item.l1_id == value) {
                this.setState({ category_l2: item.l2_list});
                break;
            }
        }
        this.setState({ searchData: searchData });
    },
    onFieldDataL2Change:function(e){
    	var select = $(':selected', e.target);//取得目前選取的option
    	var obj = this.state.fieldData;

		obj['l1_id'] = select.attr('data-l1');
		obj['l2_id'] = e.target.value;
		if(e.target.value==1){
			if(obj['product_type']==undefined)
				obj['product_type']=1;
		}
		this.setState({fieldData:obj});
    },
	render: function() {
		var outHtml = null;

		if(this.state.edit_type==0)
		{
			var searchData = this.state.searchData;

			outHtml =
			(
			<div>
                <h3 className="title">{this.props.Caption} 列表</h3>

				<form onSubmit={this.handleSearch}>
					
						<div className="table-header">
							<div className="table-filter">
								<div className="form-inline">
									<div className="form-group">

										<label>產品名稱</label> { }
										<input type="text" className="form-control input-sm" 
										value={searchData.name}
										onChange={this.changeGDValue.bind(this,'name')}
										placeholder="產品名稱..." /> { }

										<label>主分類</label> { }
										<select className="form-control input-sm" 
												value={searchData.l1_id}
												onChange={this.onL1Change.bind(this)}>
											<option value="">全部</option>
										{
											this.state.category_l1.map(function(itemData,i) {
												return <option key={i} value={itemData.l1_id}>{itemData.l1_name}</option>
											})
										}
										</select> { }

										<label>次分類</label> { }
										<select className="form-control input-sm"
												id="search-l2"
												value={searchData.l2_id}
												onChange={this.changeGDValue.bind(this,'l2_id')}>
											<option value="">全部</option>
										{
											this.state.category_l2.map(function(itemData,i) {
												return <option key={i} value={itemData.l2_id}>{itemData.l2_name}</option>
											})
										}
										</select> { }

										<label>狀態</label> { }
										<select className="form-control input-sm" 
												value={searchData.i_Hide}
												onChange={this.changeGDValue.bind(this,'i_Hide')}>
											<option value="">全部</option>
											<option value="true">隱藏</option>
											<option value="false">顯示</option>

										</select> { }


										<button className="btn-primary" type="submit"><i className="fa-search"></i>{ }搜尋</button>
									</div>
								</div>
							</div>
						</div>
						<table className="table-condensed">
							<thead>
								<tr>
									<th className="col-xs-1 text-center">
										<label className="cbox">
											<input type="checkbox" checked={this.state.checkAll} onChange={this.checkAll} />
											<i className="fa-check"></i>
										</label>
									</th>
									<th className="col-xs-1 text-center">修改</th>
									<th className="col-xs-2">主分類名稱</th>
									<th className="col-xs-3">次分類名稱</th>
									<th className="col-xs-3">產品名稱</th>
									<th className="col-xs-1">排序</th>
									<th className="col-xs-1">狀態</th>
								</tr>
							</thead>
							<tbody>
								{
								this.state.gridData.rows.map(function(itemData,i) {
								return <GridRow 
								key={i}
								ikey={i}
								primKey={itemData.product_id} 
								itemData={itemData} 
								delCheck={this.delCheck}
								updateType={this.updateType}								
								/>;
								}.bind(this))
								}
							</tbody>
						</table>
					<GridNavPage 
					StartCount={this.state.gridData.startcount}
					EndCount={this.state.gridData.endcount}
					RecordCount={this.state.gridData.records}
					TotalPage={this.state.gridData.total}
					NowPage={this.state.gridData.page}
					onQueryGridData={this.queryGridData}
					InsertType={this.insertType}
					deleteSubmit={this.deleteSubmit}
					/>
				</form>
			</div>
			);
		}
		else if(this.state.edit_type==1 || this.state.edit_type==2)
		{
			var fieldData = this.state.fieldData;
			var product_type_html=null;
			if(fieldData.l2_id==1){
				product_type_html=
					<div className="form-group">
						<label className="col-xs-2 control-label">電感分類</label>
						<div className="col-xs-4">
							<select className="form-control" 
							value={fieldData.product_type}
							onChange={this.changeFDValue.bind(this,'product_type')}>
							<option value="1">SMD 繞線式功率電感</option>
							<option value="2">一體式電感</option>
							</select>
						</div>
						<small className="help-inline col-xs-6 text-danger">(必填)</small>
					</div>;
			}
			outHtml=(
			<div>
                <h3 className="title">{this.props.Caption} 編輯</h3>

				<form className="form-horizontal clearfix" onSubmit={this.handleSubmit}>
				<div className="col-xs-9">
					<div className="form-group">
						<label className="col-xs-2 control-label">產品分類</label>
						<div className="col-xs-4">
							<select className="form-control" 
							value={fieldData.l2_id}
							onChange={this.onFieldDataL2Change.bind(this)}>
							{
								this.state.category_l1.map(function(itemData,i) {
									var l1_out_html=
									<optgroup key={itemData.l1_id} label={itemData.l1_name}>
									{
										itemData.l2_list.map(function(L2Data,i) {
											return <option key={L2Data.l2_id} data-l1={itemData.l1_id} value={L2Data.l2_id}>{L2Data.l2_name}</option>;
										}.bind(this))
									}
									</optgroup>;
									return l1_out_html;
								}.bind(this))
							}
							</select>
						</div>
						<small className="help-inline col-xs-6 text-danger">(必填)</small>
					</div>
					{product_type_html}
					<div className="form-group">
						<label className="col-xs-2 control-label">產品名稱</label>
						<div className="col-xs-4">
							<input type="text" 							
							className="form-control"	
							value={fieldData.product_name}
							onChange={this.changeFDValue.bind(this,'product_name')}
							maxLength="64" />
						</div>
					</div>
					<div className="form-group">
                        <label className="col-xs-2 control-label">產品列表圖</label>
                        <div className="col-xs-4">
                            <MasterImageUpload
                            FileKind="Photo1"
                            MainId={fieldData.product_id}
                            ParentEditType={this.state.edit_type}
                            url_upload={gb_approot + 'Active/ProductData/axFUpload'}
                            url_list={gb_approot+'Active/ProductData/axFList'}
                            url_delete={gb_approot+'Active/ProductData/axFDelete'}
                            url_sort={gb_approot+'Active/ProductData/axFSort'}
                            />
                        </div>
                        <small className="help-inline col-xs-5 text-danger">限 1 張圖片，檔案大小不可超過4.8MB</small>
                    </div>
					<div className="form-group">
						<label className="col-xs-2 control-label">排序</label>
						<div className="col-xs-4">
							<input type="number" 
							className="form-control"	
							value={fieldData.sort}
							onChange={this.changeFDValue.bind(this,'sort')} />
						</div>
						<small className="col-xs-2 help-inline">數字越大越前面</small>
					</div>
					<div className="form-group">
						<label className="col-xs-2 control-label">狀態</label>
						<div className="col-xs-4">
							<div className="radio-inline">
								<label>
									<input type="radio" 
											name="i_Hide"
											value={true}
											checked={fieldData.i_Hide===true} 
											onChange={this.changeFDValue.bind(this,'i_Hide')}
									/>
									<span>隱藏</span>
								</label>
							</div>
							<div className="radio-inline">
								<label>
									<input type="radio" 
											name="i_Hide"
											value={false}
											checked={fieldData.i_Hide===false} 
											onChange={this.changeFDValue.bind(this,'i_Hide')}
											/>
									<span>顯示</span>
								</label>
							</div>
						</div>
					</div>

					<h4 className="title row">
						<span className="col-xs-2 text-right">產品說明</span>
						<small className="col-xs-10">可不填，有填寫才會顯示</small>
					</h4>

                    <div className="form-group">
                        <label className="col-xs-2 control-label">產品說明插圖</label>
                        <div className="col-xs-4">
                            <MasterImageUpload
                            FileKind="Photo2"
                            MainId={fieldData.product_id}
                            ParentEditType={this.state.edit_type}
                            url_upload={gb_approot + 'Active/ProductData/axFUpload'}
                            url_list={gb_approot+'Active/ProductData/axFList'}
                            url_delete={gb_approot+'Active/ProductData/axFDelete'}
                            url_sort={gb_approot+'Active/ProductData/axFSort'}
                            />
                        </div>
                        <small className="help-inline col-xs-5 text-danger">限 2 張圖片，每張圖片檔案大小不可超過4.8MB</small>
                    </div>
					<div className="form-group">
						<label className="col-xs-2 control-label">產品說明</label>
						<div className="col-xs-10">
							<textarea col="30" rows="3" className="form-control" id="editor1"
							value={fieldData.product_content}
							onChange={this.changeFDValue.bind(this,'product_content')}
							maxLength="256"></textarea>
						</div>
					</div>

					<div className="form-action text-right">
						<button type="submit" className="btn-primary" name="btn-1"><i className="fa-check"></i> 儲存</button> { }
						<button type="button" onClick={this.noneType}><i className="fa-times"></i> 回前頁</button>
					</div>
				</div>
				</form>
			</div>
			);
		}else{
			outHtml=(<span>No Page</span>);
		}

		return outHtml;
	}
});